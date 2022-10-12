﻿using MediatR;

namespace DevFreela.Application.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<int>
    {
        public CreateUserCommand(string fullName, string password, string email, DateTime birthDate, IEnumerable<int>? skills)
        {
            FullName = fullName;
            Password = password;
            Email = email;
            BirthDate = birthDate;
            Skills = Enumerable.Empty<int>();

            if (skills != null)
            {
                foreach (var skill in skills)
                {
                    Skills = Skills.Append(skill);
                }
            }
        }

        public string FullName { get; private set; }

        public string Password { get; private set; }

        public string Email { get; private set; }

        public DateTime BirthDate { get; private set; }

        public IEnumerable<int>? Skills { get; private set; }
    }
}
