using PersonRepository.Interfaces;
using PersonRepository.Entities;
using System;
using PersonRepository;

namespace PersonValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            var waisburd = new Waisburd();

            var test = new ValidatorTest();
            test.Validate(waisburd);
        }
    }
}
