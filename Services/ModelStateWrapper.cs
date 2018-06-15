using Services.Interfaces;
using System.Web.Mvc;

namespace Services
{
    public class ModelStateWrapper : IValidationDictionary
    {
        #region Constructors and fields
        private readonly ModelStateDictionary modelState;

        public ModelStateWrapper(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }
        #endregion

        public void AddError(string key, string errorMessage)
        {
            this.modelState.AddModelError(key, errorMessage);
        }

        public bool IsValid
        {
            get
            {
                return this.modelState.IsValid;
            }
        }

    }
}