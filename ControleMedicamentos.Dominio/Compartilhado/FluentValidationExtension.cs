using FluentValidation.Results;
using FluentValidation;
using System.Runtime.CompilerServices;

namespace ControleMedicamento.Dominio.Compartilhado
{
    public static class FluentValidationExtension
    {
        public static bool Equals(this ValidationResult validation, ValidationResult validation1)
        {            
            if (validation.Errors.Count != validation.Errors.Count)
                return false;            
            for (int i = 0; i < validation1.Errors.Count; i++)
            {
                if (validation.Errors[i].ErrorMessage != validation1.Errors[i].ErrorMessage)
                    return false;
            }
            return true;            
        }
        public static bool Equals(ValidationResult validation, string[] validation1)
        {
            if (validation.Errors.Count != validation1.Length)
                return false;
            for (int i = 0; i < validation1.Length; i++)
            {
                if (validation.Errors[i].ErrorMessage != validation1[i])
                    return false;
            }
            return true;
        }
    }
}
