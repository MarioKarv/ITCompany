using ITCompany.Models;
using System.Linq;

namespace ITCompany.Data
{
    public class DBInitializer
    {
        public static void Initialize(ITCompanyContext context)
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Roles.Any())
            {
                return;
            }

            //Ролі
            var roles = new Role[]
            {
                new Role{ Name = "HR Manager"},
                new Role{ Name = "Директор"},
                new Role{ Name = "Клієнт"},
                new Role{ Name = "Java Software Engineering"},
                new Role{ Name = "Software Engineer"},
                new Role{ Name = "Enterprise Application Software Engineer"},
                new Role{ Name = "Sr. Full Stack Software Engineer (JavaScript)"},
                new Role{ Name = "VP of Technology"},
                new Role{ Name = "JavaScript Engineer"}, 
                new Role{ Name = "Chief Technology Officer"},
                new Role{ Name = "Senior Full Stack Developer (HTML5)"},
                new Role{ Name = "Software Engineer/Web Application Developer"},
                new Role{ Name = "iOS Developer"},
                new Role{ Name = "Senior Software Developer"},
                new Role{ Name = "Software Developer"},
                new Role{ Name = "User Interface Developer"},
                new Role{ Name = "Senior Full-Stack Engineer - Ruby on Rails (Technical Lead)"},
                new Role{ Name = "Frontend Software Engineer (Content Authoring Tools)"},
                new Role{ Name = "Senior Backend Engineer (Architecture & Maintenance)"},
                new Role{ Name = "DevOps Engineer"},
                new Role{ Name = "Lead Frontend Developer"},
                new Role{ Name = "Lead PHP Backend Developer"},
                new Role{ Name = "Senior Frontend Engineer"},
                new Role{ Name = "Senior Backend Engineer (Feature Development)"},
                new Role{ Name = "Senior PHP Backend Developer"},
                new Role{ Name = "VP of Engineering"},
                new Role{ Name = "Experienced Security People (SF)"},
                new Role{ Name = "Web Applications Developer"},
                new Role{ Name = "Android Mobile Engineer"},
                new Role{ Name = "Software Engineer / Ruby on Rails Developer"},
                new Role{ Name = "Python/JavaScript software developer in open source project"},
                new Role{ Name = "SOFTWARE-INGENIEUR / E-COMMERCE (80 – 100 %)"},
                new Role{ Name = "Cloud Engineer"},
                new Role{ Name = "Software Build Engineer"},
                new Role{ Name = "Software Engineer"},
                new Role{ Name = "Engineering Manager"},
                new Role{ Name = "Software Engineer"},
                new Role{ Name = "Site Reliability Engineer - 33891662"},
                new Role{ Name = "Frontend Specialist  "},
                new Role{ Name = "Senior Java/Android Developer"},
                new Role{ Name = "Senior Software Engineer"},
                new Role{ Name = "DevOps Engineer"},
                new Role{ Name = "Content Delivery Interconnections Manager"},
                new Role{ Name = "Lean/Agile Coach"},
                new Role{ Name = "Software Engineer | Maps & Geo"},
                new Role{ Name = "Unix System Administrator"},
                new Role{ Name = "Unix System Administrator"},
                new Role{ Name = "iOS Developer"},
                new Role{ Name = "Team Lead Development"},
                new Role{ Name = "Perl Developer"},
                new Role{ Name = "Experienced Software Developer - willing to learn Perl"},
                new Role{ Name = "Developer (Ruby on Rails)"},
                new Role{ Name = "Senior Front End Developer"}
            };

            foreach (Role r in roles)
            {
                context.Roles.Add(r);
            }

            context.SaveChanges();

            var users = new User[]
            {
                new User{ FirstName = "Олександр", LastName = "Іванов", Login = "director", Password= "director", RoleID = 29, Avatar = "/img/men_avatar.png"},
                new User{ FirstName = "Олег", LastName = "Карпенко", Login = "client1", Password= "client1", RoleID = 30, Avatar = "/img/men_avatar.png"},
                new User{ FirstName = "Тарас", LastName = "Шевченко", Login = "client2", Password= "client2", RoleID = 30, Avatar = "/img/men_avatar.png"},
                new User{ FirstName = "Ігор", LastName = "Войтенко", Login = "worker1", Password= "worker1", RoleID = 4, Avatar = "/img/men_avatar.png"},
                new User{ FirstName = "Станіслав", LastName = "Саторі", Login = "worker2", Password= "worker2", RoleID = 5, Avatar = "/img/men_avatar.png"},
                new User{ FirstName = "Георгій", LastName = "Кожум'яка", Login = "worker3", Password= "worker3", RoleID = 6, Avatar = "/img/men_avatar.png"},
                new User{ FirstName = "Юрій", LastName = "Тригуб", Login = "worker4", Password= "worker4", RoleID = 7, Avatar = "/img/men_avatar.png"},
                new User{ FirstName = "Іван", LastName = "Майданович", Login = "worker5", Password= "worker5", RoleID = 8, Avatar = "/img/men_avatar.png"},
                new User{ FirstName = "Валентин", LastName = "Будько", Login = "worker6", Password= "worker6", RoleID = 9, Avatar = "/img/men_avatar.png"},
                new User{ FirstName = "Степан", LastName = "Гайдаржи", Login = "worker7", Password= "worker7", RoleID = 10, Avatar = "/img/men_avatar.png"},
                new User{ FirstName = "Микола", LastName = "Вересень", Login = "worker8", Password= "worker8", RoleID = 11, Avatar = "/img/men_avatar.png"},
                new User{ FirstName = "Григорій", LastName = "Сковорода", Login = "worker9", Password= "worker9", RoleID = 12, Avatar = "/img/men_avatar.png"},
                new User{ FirstName = "Олександр", LastName = "Крисюн", Login = "worker10", Password= "worker10", RoleID = 13, Avatar = "/img/men_avatar.png"},
                new User{ FirstName = "Дмитро", LastName = "Самойлов", Login = "worker11", Password= "worker11", RoleID = 14, Avatar = "/img/men_avatar.png"},
                new User{ FirstName = "Андрій", LastName = "Дімовчі", Login = "worker12", Password= "worker12", RoleID = 15, Avatar = "/img/men_avatar.png"},
                new User{ FirstName = "Олексій", LastName = "Михайлов", Login = "hrmanager", Password= "hrmanager", RoleID = 1, Avatar = "/img/men_avatar.png"}
            };

            foreach(User u in users){
                context.Users.Add(u);
            }
            context.SaveChanges();


            var projectTypes = new ProjectType[]
            {
                new ProjectType{ Name = "Web програмування"},
                new ProjectType{ Name = "Бази даних"},
                new ProjectType{ Name = "Мобільні додатки"},
                new ProjectType{ Name = "Розробка веб сайтів"},
                new ProjectType{ Name = "Сторення візиток та плакатів"}
            };

            foreach (ProjectType pt in projectTypes)
            {
                context.ProjectTypes.Add(pt);
            }

            context.SaveChanges();
        }
    }
}
