using FakeAtlas.Models.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FakeAtlas.Services
{
    public class Validator: AbstractValidator<BookingUser>
    {
        public Validator()
        {
            RuleFor(user => user.FullName).NotEmpty().WithMessage("ФИО должно состоять из 3 и более символов").Length(3, 40).WithMessage("ФИО должно состоять из 3 и более символов");
        }
    }

    public class ValidatorAddress : AbstractValidator<UniqueAddress>
    {
        public ValidatorAddress()
        {
            // Rule fo check UniqueAddress.City is not empty and start with capital letter and length is between 3 and 15
            RuleFor(address => address.City).NotEmpty().Length(3, 15).Must(city => city != null && city.Length > 0 && char.IsUpper(city[0])).WithMessage("Город должен начинаться с заглавной буквы");

            
            // Rule for check UniqueAddress.Oblast is not empty and start with capital letter and length is between 3 and 12
            RuleFor(address => address.Oblast).NotEmpty().Length(3, 12).Must(oblast => oblast != null && oblast.Length > 0 && char.IsUpper(oblast[0])).WithMessage("Область должна начинаться с заглавной буквы");

            // Rule for check UniqueAddress.BuildingNum more than 0 and less than 100
            RuleFor(address => address.BuildingNum).InclusiveBetween(1, 100);

            // Rule for check UniqueAddress.Street is not empty and start with capital letter and length is between 3 and 20
            RuleFor(address => address.StreetName).NotEmpty().Length(3, 20).WithMessage("Улица должна состоять из 3 и более символов").Must(street => street != null && street.Length > 0 && char.IsUpper(street[0])).WithMessage("Улица должна начинаться с заглавной буквы");
           
        }
    }

    public class ValidatorPassword: AbstractValidator<string>
    {
        public ValidatorPassword()
        {
            // Rule for check password length more than 8 and less than 30, must contain at least one digit and one letter and one upper case letter
            RuleFor(password => password).Length(8, 30).WithMessage("Пароль должен содержать от 8 до 30 символов")
                .Must(password => password.Any(char.IsDigit)).WithMessage("Пароль должен содержать хотя бы одну цифру")
                .Must(password => password.Any(char.IsUpper)).WithMessage("Пароль должен содержать хотя бы одну заглавную букву")
                .Must(password => password.Any(char.IsLetter)).WithMessage("Пароль должен содержать хотя бы одну букву");
        }
    }

    public class ValidatorEmail: AbstractValidator<string>
    {
        public ValidatorEmail()
        {
            // Rule for check email
            RuleFor(email => email).EmailAddress().WithMessage("Некорректный email");

            // Email should contain @ and .
            RuleFor(email => email).Must(email => email.Contains("@")).WithMessage("Некорректный email")
                .Must(email => email.Contains(".")).WithMessage("Некорректный email");

            // Check email with regexp
            RuleFor(email => email).Must(email => Regex.IsMatch(email, @"^[a-zA-Z0-9_\-\.]+@[a-zA-Z0-9\-]+\.[a-zA-Z0-9\-\.]+$")).WithMessage("Некорректный email");
            
        }
    }
    
    public class ValidatorPath: AbstractValidator<string>
    {

        public ValidatorPath()
        {
            // Min 5 symbols max 15 symbols
            RuleFor(path => path).Length(5, 15).WithMessage("Неверно введен Город");
            //notnull
            RuleFor(path => path).NotNull().WithMessage("Поле ввода не должно быть пустым");
        }
    }


}
