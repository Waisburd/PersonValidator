using PersonRepository.Entities;
using PersonRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonRepository
{
    public class ValidatorTest
    {
        public void Validate(IPersonRepositoryBasic personRepository)
        {
        }

        private void ValidateAdd(IPersonRepositoryBasic personRepository)
        {
            Console.WriteLine("Validando add...");

            personRepository.People = new List<Person>();

            var personOk = new Person()
            {
                Id = 1,
                Name = "Ariel",
                Age = 14,
                Email = "ariel@ariel.com",
            };

            personRepository.Add(personOk);

            if (!personRepository.People.Any(p => p.Id == 1 && p.Name == "Ariel"))
            {
                throw new Exception("No se encuentra la persona insertada");
            }

            personRepository.Add(new Person()
            {
                Id = 2,
                Email = "ariel@ariel.com",
                Age = 14,
            });


            personRepository.Add(new Person()
            {
                Id = 2,
                Email = "ariel@ariel.com",
                Age = 14,
            });

            if (personRepository.People.Count(p => p.Id == 2) != 1)
            {
                throw new Exception("Permitio repetir ids");
            }



        }
    }
}
