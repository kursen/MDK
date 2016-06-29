using System;
using System.Globalization;
using System.Web.Mvc;

namespace HaloUIHelpers.Helpers
{
    public class DecimalModelBinder: IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
        ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                actualValue = Convert.ToDecimal(valueResult.AttemptedValue,
                    CultureInfo.CurrentCulture);
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
    public class DoubleModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
        ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                actualValue = Convert.ToDouble(valueResult.AttemptedValue,
                    CultureInfo.CurrentCulture);
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }

    public class SingleModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
        ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                actualValue = Convert.ToSingle(valueResult.AttemptedValue,
                    CultureInfo.CurrentCulture);
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
    public class IntegerModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
        ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                if (valueResult != null)
                {
                    string groupseparator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
                    string tempresult = valueResult.AttemptedValue.Replace(groupseparator, string.Empty);

                    actualValue = Int32.Parse(tempresult, CultureInfo.CurrentCulture);
                }
              
                
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }

        
    }
    public class LongModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
        ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                if (valueResult != null)
                {
                    string groupseparator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
                    string tempresult = valueResult.AttemptedValue.Replace(groupseparator, string.Empty);
                    actualValue = Int64.Parse(tempresult, CultureInfo.CurrentCulture);
                }
               
                
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}
