using BeautySalon.Models.DataBase;
using FluentValidation;

namespace BeautySalon.Validators
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator() 
        { 

        }
    }
}
