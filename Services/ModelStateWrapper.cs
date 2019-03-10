using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services.Interfaces;

namespace Services
{
    public class ModelStateWrapper : IValidationDictionary
    {
        #region Constructors and fields
        public ModelStateDictionary ModelStateDictionary { get; private set; }
        public ModelStateWrapper(ModelStateDictionary modelState)
        {
            this.ModelStateDictionary = modelState;
        }
        #endregion

        public void AddError(string key, string errorMessage)
        {
            this.ModelStateDictionary.AddModelError(key, errorMessage);
        }

        public bool IsValid
        {
            get
            {
                return this.ModelStateDictionary.IsValid;
            }
        }

    }
}