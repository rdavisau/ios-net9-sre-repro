using System.Reflection;
using System.Reflection.Emit;

namespace SRE;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
    public override UIWindow? Window { get; set; }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        Window = new UIWindow
            { RootViewController = new UIViewController { View = { BackgroundColor = UIColor.Green } } };
        Window.MakeKeyAndVisible();

        Console.WriteLine("About to emit down");

        EmitDown();

        Console.WriteLine("I just emitted down");

        return true;
    }

    void EmitDown()
    {
        var temp = Path.GetTempFileName() + ".dll";
        var name = Path.GetFileName(temp);
        var aname = new AssemblyName("Emit");
        var ab = AssemblyBuilder.DefineDynamicAssembly(aname, AssemblyBuilderAccess.Run);
        var mod = ab.DefineDynamicModule(name);
        var tb = mod.DefineType("DynamicClass", TypeAttributes.Public);

        var mb = tb.DefineMethod("Print", MethodAttributes.Public, null, new[] { typeof(string) });
        var il = mb.GetILGenerator();
        il.EmitWriteLine("Hello Code Generation!");
        il.Emit(OpCodes.Ret);

        var mb2 = tb.DefineMethod("Add", MethodAttributes.Public, typeof(int), new[] { typeof(int), typeof(int) });
        var il2 = mb2.GetILGenerator();
        il2.Emit(OpCodes.Ldarg_1);
        il2.Emit(OpCodes.Ldarg_2);
        il2.Emit(OpCodes.Add);
        il2.Emit(OpCodes.Ret);

        var t = tb.CreateType();

        var it = Activator.CreateInstance(t);
        t.GetMethod("Print").Invoke(it, new[] { "🫨" });
    }
}