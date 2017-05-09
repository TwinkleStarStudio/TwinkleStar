using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using TwinkleStar.Data.JsonRepository;
using TwinkleStar.Data.XMLRepository;
using TwinkleStar.Demo.Core;
using TwinkleStar.Demo.Core.Data.Initialize;
using TwinkleStar.Demo.Core.Data.Repositories;
using TwinkleStar.Demo.Core.Models;


namespace TwinkleStar.Test
{
    [Export]
    class Program
    {
        private static CompositionContainer _container;

        [Import]
        public IAccountService AccountService { get; set; }

        static void Main(string[] args)
        {
            try
            {
                //IJsonRepository<Student> jsonRepository = new JsonRepository<Student>();
                //jsonRepository.IsIndent = true;

                //for (int i = 5; i < 8; i++)
                //{
                //    Student stu = new Student();
                //    stu.SID = 1;
                //    stu.SName = "涂小丽";
                //    stu.Age = i;
                //    stu.Gender = "female";
                //    stu.Age = i;
                //    jsonRepository.Insert(stu);

                //    //Student stud = null;

                //    //stud.Age = i * i;
                //    //jsonRepository.Update(stud);

                //    //jsonRepository.Delete(stud);
                //}

                //foreach (var item in jsonRepository.GetModel().ToList())
                //{
                //    Console.Write(item.RootID);
                //    Console.Write("\t");
                //    Console.Write(item.SName);
                //    Console.Write("\t");
                //    Console.Write(item.SID);
                //    Console.Write("\t");
                //    Console.Write(item.Gender);
                //    Console.Write("\t");
                //    Console.Write(item.Age);
                //    Console.Write("\t");
                //    Console.Write(item.ClassNum);
                //    Console.Write("\t");
                //    Console.WriteLine();
                //}

                //Console.WriteLine(jsonRepository.Count());

                AggregateCatalog catalog = new AggregateCatalog();
                catalog.Catalogs.Add(new DirectoryCatalog(Directory.GetCurrentDirectory()));
                catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
                _container = new CompositionContainer(catalog);
                new DemoDatabaseInitializer().Initialize();

                IMemberRepository memberRepository = _container.GetExportedValue<IMemberRepository>();
                IRoleRepository roleRepository = _container.GetExportedValue<IRoleRepository>();

                List<Member> members = memberRepository.GetModel().ToList();

                foreach (var member in members)
                {
                    Console.WriteLine(member.Id + ":" + member.Roles.FirstOrDefault().RoleType);
                }

                Member m = new Member();
                m.Age = 1;
                m.NickName = "二狗";
                m.Password = "123456";
                m.Email = "weqeqe";
                m.UserName = "涂二狗";
                m.Roles.Add(new Role() { Description = "hello", RoleType = RoleType.Admin, Name = "管理员" });

                //memberRepository.Insert(m);


                //memberRepository.Delete(members);

                //Logger loger = Logger.GetLogger("Test");

                //loger.Info("Test");

                //string[] strArr = { "string", "huang", "Toto"};
                //List<string> lst = new List<string>();
                //lst.AddRange(strArr);

                //if (lst.Contains("STRING", new Common.Others.StringComparisonIgnoreCase<String>()))
                //{
                //    Console.WriteLine("contains");
                //}
                //else
                //{
                //    Console.WriteLine("not contains");
                //}

                //IRepository<Student> xml = new XML2Repository<Student>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(Student).Name + ".xml"));

                //for (int i = 5; i < 8; i++)
                //{
                //    //Student stu = new Student();
                //    //stu.SID = 1;
                //    //stu.SName = "涂小丽";
                //    //stu.Age = i;
                //    //stu.Gender = "female";
                //    //stu.Age = i;
                //    //xml.Insert(stu);


                //    Student stud = null;

                //    //stud.Age = i * i;
                //    //xml.Update(stud);

                //    xml.Delete(stud);
                //}


                //XMLHelper.CreateXML<Student>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(Student).Name + ".xml"), stu);

                //DateTime dt = new DateTime(2016, 8, 1, 1, 0, 0);
                //DateTime dtEnd = new DateTime(2016, 8, 1, 14, 0, 0);

                //foreach (var item in Enum.GetValues(typeof(DateType)))
                //{
                //    Console.WriteLine((DateType)item + ":" + dt.DateDiffRound(dtEnd, (DateType)item));
                //}

                //Console.WriteLine(dt.DateDiffString(dtEnd, false));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            Console.ReadKey();
        }

    }

    public class Student : JsonEntity
    {
        public int SID { get; set; }
        public string SName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public int ClassNum { get; set; }
    }
}
