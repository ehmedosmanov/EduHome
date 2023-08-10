using Microsoft.AspNetCore.Identity;

namespace EduHome3.Helpers
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUniqueChars),
                Description = $"Parol-da en azı {uniqueChars} unikal simbol movcud olmalidir."
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = $"Parol en azi {length} simboldan ibaret olmalidir."
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = "en azi 1 kicik simbol olmalidir"
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "en azi 1 boyuk simbol olmalidir"
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "xususi simbol olmalidir"
            };
        }
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = $"Bele bir {userName} movcud deyildir."
            };
        }
        //public override IdentityError Lockout()
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(Lockout),
        //        Description = "Превышено максимальное количество неудачных попыток входа. Попробуйте снова через некоторое время."
        //    };
        //}
    }
}
