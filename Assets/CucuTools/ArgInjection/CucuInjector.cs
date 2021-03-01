using System;
using System.Linq;
using System.Reflection;

namespace CucuTools
{
    public static class CucuInjector
    {
        public static void Inject(object target, params CucuArg[] cucuArgs)
        {
            if (target == null) return;
            
            var targetType = target.GetType();
            
            var fields = targetType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var argFields = fields.Where(f => CustomAttributeExtensions.GetCustomAttribute<InjectArgAttribute>((MemberInfo) f) != null).ToArray();

            foreach (var argField in argFields)
            {
                var fieldType = argField.FieldType;

                if (fieldType.IsArray)
                {
                    var args = cucuArgs.Where(ca => ca.GetType() == fieldType.GetElementType()).ToArray();

                    if ((args?.Length ?? 0) > 0)
                    {
                        var array = Array.CreateInstance(fieldType.GetElementType(), args.Length);

                        Array.Copy(args, array, args.Length);

                        argField.SetValue(target, array);
                    }
                }
                else
                {
                    var arg = cucuArgs.FirstOrDefault(ca => ca.GetType() == argField.FieldType);
                    if (arg != null)
                    {
                        argField.SetValue(target, Convert.ChangeType(arg, argField.FieldType));
                    }
                }
            }
        }
    }
}