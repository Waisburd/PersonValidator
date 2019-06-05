using System;
using System.Collections.Generic;
using PersonRepository.Entities;
using PersonRepository.Interfaces;
using System.Linq;
using System.Linq.Expressions;

class Waisburd : IValidatorExpert
{
    public List<Person> People { get; set; }

    public void Add(Person person)
    {
        if(People == null)
            People = new List<Person>();

        if(ValidarId(person) && ValidarEmail(person) && ValidarAge(person))
            People.Add(person);
    }

    public void Delete(int id)
    {
        People.Remove(People.First(p => p.Id == id));
    }

    public int GetCountRangeAges(int min, int max)
    {
        return People.Count(p => p.Age > min && p.Age < max);
        // int count = 0;
        // foreach(var p in People)
        // {
        //     if (p.Age > min && p.Age < max)
        //         count++;
        // }
        // return count;
    }

    public List<Person> GetFiltered(string name, int age, string email)
    {
        Func<Person, bool> nameFilter = (p => (!string.IsNullOrEmpty(name)) ?  p.Name == name : true);
        Func<Person, bool> ageFilter = (p => ((age != 0)) ?  p.Age == age : true);
        Func<Person, bool> emailFilter = (p => (!string.IsNullOrEmpty(email)) ?  p.Email.Contains(email) : true);

        return People.Where(p => nameFilter(p) && ageFilter(p) && emailFilter(p)).ToList();

        // List<Person> list = new List<Person>();
        // bool nameFilter = (name != null) && (name != string.Empty);
        // bool ageFilter = (age != 0);
        // bool emailFilter = (email != null) && (email != string.Empty);

        // foreach(var p in People)
        // {
        //     if(!(nameFilter && p.Name != name) && !(ageFilter && p.Age != age) && !(emailFilter && !p.Email.Contains(email)))
        //         list.Add(p);
        // }
        // return list;
    }

    public Person GetPerson(int Id)
    {
        return People.Find(p => p.Id == Id);
    }

    public void Update(Person person)
    {
        if(People.Exists(CompareId(person)) && ValidarAge(person) && ValidarEmail(person))
        {
            People.RemoveAt(People.FindIndex(CompareId(person)));
            People.Add(person);
        }
        // if(!ValidarId(person, People) && person.Age > 0 && ValidarAge(person))
        // {
        //     foreach(var p in People)
        //     {
        //         if(p.Id == person.Id)
        //         {   
        //             People.Remove(p);
        //             break;
        //         }
        //     }
        //     Add(person);
        // }
    }

    private bool ValidarEmail(Person person)
    {
        return person.Email.Contains('@') && person.Email.Contains('.');
    }
    private bool ValidarId(Person person)
    {
        return People.Count(p => p.Id == person.Id) == 0;
    }
    private bool ValidarAge(Person person)
    {
        return (person.Age > 0) ? true : false;
    }

    public Predicate<Person> CompareId(Person person)
    {
        return p => p.Id == person.Id;
    }

    private string[] WordsInName(Person person)
    {
        return person.Name.Split(" ");
    }
    public int[] GetNotCapitalizedIds()
    {
        Func<string, bool> wordIsLower = word => char.IsLower(word.ToCharArray()[0]);
        IEnumerable<Person> notCapitalizedPeople = People.Where(p => WordsInName(p).Where(wordIsLower).Count() > 0);
        IEnumerable<int> ids = notCapitalizedPeople.Select(p => p.Id);

        return ids.ToArray();
    }

    public Dictionary<int, string[]> GroupEmailByNameCount()
    {
        const int MAX_NAMES = 6;
        Dictionary<int, string[]> groups = new Dictionary<int, string[]>();
        for(var i = 0; i <= MAX_NAMES; i++)
        {
            string[] emails = People.Where(p => WordsInName(p).Count() == i).Select(person => person.Email).ToArray();
            groups.Add(i, emails);
        }
        return groups;
    }

    public bool Run(Person person, Expression<Func<Person, bool>> validation)
    {
        Func<Person, bool> validate = validation.Compile();
        return validate(person);
    }
}
