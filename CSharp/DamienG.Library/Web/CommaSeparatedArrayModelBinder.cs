using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace DamienG.Web
{
    /// <summary>
    /// Allows for integers to be passed on the URL as comma-seperated values in addition to the usual
    /// multi-named values (i.e.  param=1,2,3 instead of just param=1&amp;param=2&amp;param=3).
    /// </summary>
    public class CommaSeparatedArrayModelBinder : IModelBinder
    {
        private static readonly Type[] supportedElementTypes =
        {
            typeof(int), typeof(long), typeof(short), typeof(byte),
            typeof(uint), typeof(ulong), typeof(ushort)
        };

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (!IsSupportedModelType(bindingContext.ModelType))
                return false;

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var stringArray = valueProviderResult?.AttemptedValue?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (stringArray == null)
                return false;

            var elementType = bindingContext.ModelType.GetElementType();
            if (elementType == null)
                return false;

            bindingContext.Model = CopyAndConvertArray(stringArray, elementType);

            return true;
        }

        private static Array CopyAndConvertArray(IReadOnlyList<string> sourceArray, Type elementType)
        {
            var targetArray = Array.CreateInstance(elementType, sourceArray.Count);

            if (sourceArray.Count > 0)
            {
                var converter = TypeDescriptor.GetConverter(elementType);
                for (var i = 0; i < sourceArray.Count; i++)
                    targetArray.SetValue(converter.ConvertFromString(sourceArray[i]), i);
            }

            return targetArray;
        }

        internal static bool IsSupportedModelType(Type modelType)
        {
            return modelType.IsArray
                   && modelType.GetArrayRank() == 1
                   && modelType.HasElementType
                   && supportedElementTypes.Contains(modelType.GetElementType());
        }

    }

    public class CommaSeparatedArrayModelBinderProvider : ModelBinderProvider
    {
        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            return CommaSeparatedArrayModelBinder.IsSupportedModelType(modelType)
                ? new CommaSeparatedArrayModelBinder()
                : null;
        }
    }
}