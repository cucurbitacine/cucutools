using System;
using System.Linq;
using System.Reflection;
using CucuTools.Attributes;

namespace CucuTools.ArgumentInjector
{
    /// <summary>
    /// Base injection logic
    /// </summary>
    public static class CucuInjector
    {
        /// <summary>
        /// Fill fields with <see cref="CucuArgAttribute"/> into <param name="target"></param> 
        /// </summary>
        /// <param name="target">Tartet</param>
        /// <param name="poolArgs">Pool of arguments</param>
        public static void Inject(object target, params CucuArg[] poolArgs)
        {
            if (target == null) return;
            
            // get type of target
            var targetType = target.GetType();
            
            // get all instance fields
            var fields = targetType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            // get fields with interested attribute
            var argFields = fields.Where(f => CustomAttributeExtensions.GetCustomAttribute<CucuArgAttribute>((MemberInfo) f) != null).ToArray();

            // for each field
            foreach (var argField in argFields)
            {
                // get field type
                var fieldType = argField.FieldType;

                // if field type array
                if (fieldType.IsArray)
                {
                    // search in pool args with interested type
                    var args = poolArgs.Where(ca => ca.GetType() == fieldType.GetElementType()).ToArray();

                    if ((args?.Length ?? 0) > 0)
                    {
                        // create array
                        var array = Array.CreateInstance(fieldType.GetElementType(), args.Length);

                        // fill array
                        Array.Copy(args, array, args.Length);

                        // set array
                        argField.SetValue(target, array);
                    }
                }
                else
                {
                    // search in pool arg with interested type
                    var arg = poolArgs.FirstOrDefault(ca => ca.GetType() == argField.FieldType);
                    
                    if (arg != null)
                    {
                        // set field
                        argField.SetValue(target, Convert.ChangeType(arg, argField.FieldType));
                    }
                }
            }
        }
    }
}