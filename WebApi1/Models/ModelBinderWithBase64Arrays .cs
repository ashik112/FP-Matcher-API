using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace WebApi1.Models
{
   
        public class ModelBinderWithBase64Arrays : IModelBinder
        {
            public bool BindModel(System.Web.Http.Controllers.HttpActionContext actionContext, ModelBindingContext bindingContext)
            {
                if (bindingContext.ModelType == typeof(byte[]))
                {
                    ValueProviderResult val = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                    string valueAsString = val.RawValue as string;
                    try
                    {
                        bindingContext.Model = Convert.FromBase64String(valueAsString);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
    
}