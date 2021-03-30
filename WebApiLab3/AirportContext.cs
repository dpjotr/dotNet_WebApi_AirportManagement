using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace AirportModel
{
    public class AirportContext : DbContext
    {

        public DbSet<Airplane> Airplanes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Trip> Trips { get; set; }


        public AirportContext()
        {

         //   Database.EnsureDeleted();
        //    Database.EnsureCreated();
         //   FillDatabase();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;Database=AirportModelDb;Trusted_Connection=TRUE;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasKey(b => new {b.TripId, b.PassengerId});

            modelBuilder.Entity<Ticket>()
                .HasOne(e => e.Passenger)
                .WithMany(m => m.Trips)
                .HasForeignKey(e => e.PassengerId);

            modelBuilder.Entity<Ticket>()
                .HasOne(e => e.Trip)
                .WithMany(m => m.Passengers)
                .HasForeignKey(e => e.TripId);



            modelBuilder.Entity<Airplane>().HasData(
                new Airplane[]
                {
                    new Airplane
                    {
                        AirplaneId = 1,
                        Manufacturer = "Boeing",
                        Model = "777",
                        ProductionDate = new DateTime(2010, 10, 21),
                        LastInspection = new DateTime(2020, 3, 29),
                        CompanyId = 1
                    },
                    new Airplane
                    {
                        AirplaneId = 2,
                        Manufacturer = "Boeing",
                        Model = "747",
                        ProductionDate = new DateTime(1989, 8, 3),
                        LastInspection = new DateTime(2020, 5, 9),
                        CompanyId = 2
                    }
                });
            modelBuilder.Entity<Passenger>().HasData(
                new Passenger[]
                {
                    new Passenger
                    {
                        PassengerId = 1,
                        Name = "Anna"
                    },
                    new Passenger
                    {
                        PassengerId = 2,
                        Name = "Bintur"
                    }
                });
            modelBuilder.Entity<Company>().HasData(
                new Company[]
                {
                    new Company
                    {
                        CompanyId = 1,
                        Name = "AirAstana",
                        Country = "Kazakhstan"

                    },
                    new Company
                    {
                        CompanyId = 2,
                        Name = "AirBaku",
                        Country = "Azerbaijan"
                    }
                });


        }

        //Basic methods
        internal void AddPassenger(String name)
        {
            if (name.Length > 0)
            {
                Passenger p = new Passenger {Name = name};
                this.Passengers.Add(p);
                this.SaveChanges();
            }
            else
                Console.WriteLine("Name can not be empty");
        }

        internal void AddPassenger()
        {
            Console.WriteLine("Enter necessary data in order to create new passenger record\n");
            Console.WriteLine("Enter passenger name\n");
            String name = Console.ReadLine();
            AddPassenger(name);
            this.SaveChanges();
            MainMenu();
        }

        internal void AddCompany(String name, String company)
        {
            if (name.Length > 0 && company.Length > 0)
            {
                Company c = new Company {Name = name, Country = company, Airplanes = new List<Airplane>()};
                this.Companies.Add(c);
                this.SaveChanges();
            }
            else
                Console.WriteLine("Name can not be empty");
        }

        internal void AddCompany()
        {
            Console.WriteLine("Enter necessary data in order to create new company record\n");
            Console.WriteLine("Enter company name\n");
            String name = Console.ReadLine();
            Console.WriteLine("Enter country of company origin\n");
            String country = Console.ReadLine();
            AddCompany(name, country);

            this.SaveChanges();
            MainMenu();
        }

        internal void AddAirplane(String manuf, String model, DateTime production, int id)
        {
            if (Companies.Find(id) == null)
            {
                Console.WriteLine("Company with such id does not exist");
                return;
            }

            if (manuf.Length > 0 && model.Length > 0)
            {
                Airplane a = new Airplane
                {
                    Manufacturer = manuf, Model = model, ProductionDate = production, LastInspection = DateTime.Now,
                    CompanyId = id
                };
                this.Airplanes.Add(a);
                this.SaveChanges();
            }
            else
                Console.WriteLine("Name can not be empty");
        }

        internal void AddAirplane()
        {
            Console.WriteLine("Enter necessary data in order to create new airplane record\n");
            Console.WriteLine("Enter manufacturer\n");
            String manuf = Console.ReadLine();
            Console.WriteLine("Enter model\n");
            String model = Console.ReadLine();
            Console.WriteLine("Enter year of production yyyy\n");
            String y = Console.ReadLine();
            DateTime production = new DateTime(int.Parse(y));

            Console.WriteLine($@"                  id     company");
            foreach (var x in this.Companies)
                Console.WriteLine($@"                  {x.CompanyId}     {x.Name}");
            Console.WriteLine("Enter company id, from the list above\n");
            String id = Console.ReadLine();
            int companyId = int.Parse(id);

            Console.WriteLine("Does airplane meet all the necessary requirements?y/n\n");
            String answer = Console.ReadLine();
            if (answer.ToUpper()[0] == 'Y')
            {
                AddAirplane(manuf, model, production, companyId);
                this.SaveChanges();
            }
            else
                Console.WriteLine("All the airplane defects have to be fixed before registration\n");

            MainMenu();
        }

        internal void AddTrip(String from, String to, DateTime takeof, DateTime arrival, int planeId)
        {
            if (arrival > takeof && Airplanes.Find(planeId) != null)
                Trips.Add(new Trip {From = from, To = to, Takeof = takeof, Arrival = arrival, AirplaneId = planeId});
            this.SaveChanges();
        }

        internal void AddTrip()
        {
            try
            {
                Console.WriteLine("Enter home airport");
                String from = Console.ReadLine();
                Console.WriteLine("Enter airport of destination");
                String to = Console.ReadLine();

                Console.WriteLine("Chose company Id from the list");
                Console.WriteLine($@"                  id     company");
                foreach (var x in this.Companies)
                    Console.WriteLine($@"                  {x.CompanyId}     {x.Name}");
                Console.WriteLine("Enter company id, from the list above\n");
                String id = Console.ReadLine();
                int companyId = int.Parse(id);

                Console.WriteLine("Chose airplane Id from the list");
                Console.WriteLine($@"                  id     Airplane");
                foreach (var x in this.Airplanes)
                    if (x.CompanyId == companyId)
                        Console.WriteLine($@"                  {x.AirplaneId}     {x.Manufacturer}  {x.Model}");
                Console.WriteLine("Enter airplane id, from the list above\n");
                String aPid = Console.ReadLine();
                int planeId = int.Parse(aPid);
                List<int> planes = new List<int>();
                foreach (var x in Airplanes)
                    if (x.CompanyId == companyId)
                        planes.Add(x.AirplaneId);

                if (!planes.Contains(planeId)) throw new Exception();


                Console.WriteLine("Enter takeof time in format yyyy-MM-dd HH:mm");
                String temp1 = Console.ReadLine();
                DateTime takeof = DateTime.ParseExact(temp1, "yyyy-MM-dd HH:mm", null);

                Console.WriteLine("Enter arrival time in format yyyy-MM-dd HH:mm");
                temp1 = Console.ReadLine();
                DateTime arrival = DateTime.ParseExact(temp1, "yyyy-MM-dd HH:mm", null);


                if (arrival > takeof && Airplanes.Find(planeId) != null)
                    AddTrip(from, to, takeof, arrival, planeId);
                else
                    Console.WriteLine("Data is not correct");
                this.SaveChanges();
                MainMenu();
            }
            catch (Exception e)
            {
                Console.WriteLine("Data input is not correct");
            }
        }

        //Menue
        public void MainMenu()
        {
            Console.WriteLine("==============Welcome_to_the_airport_management_system==============");
            Console.WriteLine("In order to add new data to the system press 1");
            Console.WriteLine("In order to view data press 2");
            Console.WriteLine("In order to delete data press 3");
            Console.WriteLine("In order to change data press 4");
            Console.WriteLine("In order to show statisticas press 5");
            int command = 0;
            try
            {
                command = int.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                MainMenu();
            }

            switch (command)
            {
                case 1:
                    AddMenu();
                    break;
                case 2:
                    ShowMenu();
                    break;
                case 3:
                    DeleetMenu();
                    break;
                case 4:
                    EditMenu();
                    break;
                case 5:
                    ShowStats();
                    break;

                default:
                    MainMenu();
                    break;
            }

        }

        private void ShowStats()
        {

            Console.WriteLine("===============Air companies============");
            foreach (var x in Companies.Include(a => a.Airplanes))
            {
                Console.WriteLine(
                    $"\n\nname:{x.Name}, country:{x.Country}\nAirplanes\n{string.Join('\n', x.Airplanes)} ");
                if (x.Airplanes == null) x.Airplanes = new List<Airplane>();
            }



            Console.WriteLine("===============Airplanes============");
            foreach (var x in Airplanes.Include(a => a.Trips))
            {
                if (x.Trips == null) x.Trips = new List<Trip>();
                String trips = x.Trips.Count == 0 ? "No flights planned" : $"{string.Join('\n', x.Trips)}";
                Console.WriteLine(
                    $"id:{x.AirplaneId}, company:{x.Company}, {x.Manufacturer}-{x.Model}\nTrips\n{trips}");
            }



        }

        private void EditMenu()
        {
            Console.WriteLine("==============Edit_data==============");
            Console.WriteLine("In order to remove or add passengers to flight press 1");
            Console.WriteLine("In order to change takeof or arrival time of a flight press 2");

            int command = int.Parse(Console.ReadLine());

            switch (command)
            {
                case 1:
                    AddRemPassengerToFlight();
                    break;
                case 2:
                    ChangeTakeofArival();
                    break;

                default:
                    Console.WriteLine("Wrong entry");
                    break;
            }

        }

        private void ChangeTakeofArival()
        {
            Console.WriteLine("Chose flight id and press enter");
            CheckTrip();
            int entry = int.Parse(Console.ReadLine());
            Console.WriteLine($@"Would you like to change takeof time of flight #{entry}");
            if (Console.ReadLine().ToUpper()[0] == 'Y')
            {
                Console.WriteLine("Enter takeof time in format yyyy-MM-dd HH:mm");
                String temp1 = Console.ReadLine();
                DateTime takeof = DateTime.ParseExact(temp1, "yyyy-MM-dd HH:mm", null);
                foreach (var x in Trips)
                    if (x.TripId == entry)
                        x.Takeof = takeof;
                this.SaveChanges();

            }

            Console.WriteLine($@"Would you like to change arrival time of flight #{entry}");
            if (Console.ReadLine().ToUpper()[0] == 'Y')
            {
                Console.WriteLine("Enter arrival time in format yyyy-MM-dd HH:mm");
                String temp1 = Console.ReadLine();
                DateTime arrival = DateTime.ParseExact(temp1, "yyyy-MM-dd HH:mm", null);
                foreach (var x in Trips)
                    if (x.TripId == entry)
                        x.Arrival = arrival;
                this.SaveChanges();

            }

            MainMenu();
        }



        private void AddRemPassengerToFlight()
        {
            Console.WriteLine("Remove/Add passenger to flight============");
            Console.WriteLine("Chose flight id and press enter");
            CheckTrip();
            int entry = int.Parse(Console.ReadLine());

            Console.WriteLine($@"Would you like to add passenger to flight #{entry}");
            if (Console.ReadLine().ToUpper()[0] == 'Y')
            {
                Console.WriteLine("Enter passenger id");
                CheckPassenger();
                int pas = int.Parse(Console.ReadLine());

                foreach (var x in Trips)
                    if (x.TripId == entry)
                    {
                        if (x.Passengers == null) x.Passengers = new List<Ticket>();
                        x.Passengers.Add(new Ticket {TripId = entry, PassengerId = pas});
                    }

                this.SaveChanges();

            }

            Console.WriteLine($@"Would you like to remove passenger from flight #{entry}");
            if (Console.ReadLine().ToUpper()[0] == 'Y')
            {
                Console.WriteLine("Enter passenger id");
                CheckPassenger();
                int pas = int.Parse(Console.ReadLine());
                var passengers = Tickets;

                foreach (var x in Trips)
                    if (x.TripId == entry)
                        x.Passengers.Remove(passengers.Where(p => p.PassengerId == pas).FirstOrDefault());
                this.SaveChanges();

            }

        }

        private void DeleetMenu()
        {
            {
                Console.WriteLine("==============Delete_data==============");
                Console.WriteLine("To delete air operator press 1");
                Console.WriteLine("To delete airplane press 2");
                Console.WriteLine("To delete passenger press 3");
                Console.WriteLine("To delete flight press 4");
                Console.WriteLine("In order to exit press 5");

                int command = int.Parse(Console.ReadLine());

                switch (command)
                {
                    case 1:
                        DelCompany();
                        break;
                    case 2:
                        DelAirplane();
                        break;
                    case 3:
                        DelPassenger();
                        break;
                    case 4:
                        DelTrip();
                        break;
                    case 5:
                        MainMenu();
                        break;
                    default:
                        Console.WriteLine("Wrong entry");
                        break;
                }

            }
        }

        private void DelPassenger()
        {
            Console.WriteLine("============ Delete Passengers:");
            CheckPassenger();
            Console.WriteLine("In order to delete passenger enter passenger id and press enter");
            int pas = int.Parse(Console.ReadLine());
            foreach (var x in Passengers)
                if (x.PassengerId == pas)
                    Passengers.Remove(x);
            this.SaveChanges();
            MainMenu();


        }

        private void DelTrip()
        {
            Console.WriteLine("============ Delete Flight:");
            CheckTrip();
            Console.WriteLine("In order to delete flight enter flight id and press enter");
            int pas = int.Parse(Console.ReadLine());
            foreach (var x in Trips)
                if (x.TripId == pas)
                    Trips.Remove(x);
            this.SaveChanges();
            MainMenu();
        }

        private void DelAirplane()
        {
            Console.WriteLine("============ Delete Airplane:");
            CheckAirplane();
            Console.WriteLine("In order to delete airplane enter airplane id and press enter");
            int pas = int.Parse(Console.ReadLine());
            foreach (var x in Airplanes)
                if (x.AirplaneId == pas)
                    Airplanes.Remove(x);
            this.SaveChanges();
            MainMenu();
        }

        private void DelCompany()
        {
            Console.WriteLine("============ Delete Company:");
            CheckCompany();
            Console.WriteLine("In order to delete flight enter flight id and press enter");
            int pas = int.Parse(Console.ReadLine());
            foreach (var x in Companies)
                if (x.CompanyId == pas)
                    Companies.Remove(x);
            this.SaveChanges();
            MainMenu();
        }

        public void AddMenu()
        {
            Console.WriteLine("==============Add_new_data==============");
            Console.WriteLine("To add new air operator press 1");
            Console.WriteLine("To add new airplane press 2");
            Console.WriteLine("To add new passenger press 3");
            Console.WriteLine("To add new flight press 4");
            Console.WriteLine("In order to exit press 5");

            int command = int.Parse(Console.ReadLine());

            switch (command)
            {
                case 1:
                    AddCompany();
                    break;
                case 2:
                    AddAirplane();
                    break;
                case 3:
                    AddPassenger();
                    break;
                case 4:
                    AddTrip();
                    break;
                case 5:
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("Wrong entry");
                    break;
            }

        }

        public void ShowMenu()
        {
            Console.WriteLine("==============Add_new_data==============");
            Console.WriteLine("To check available air operators press 1");
            Console.WriteLine("To check available airplanes press 2");
            Console.WriteLine("To get the list of passengers press 3");
            Console.WriteLine("To check existing flights press 4");
            Console.WriteLine("In order to exit press 5");

            int command = int.Parse(Console.ReadLine());

            switch (command)
            {
                case 1:
                    CheckCompany();
                    break;
                case 2:
                    CheckAirplane();
                    break;
                case 3:
                    CheckPassenger();
                    break;
                case 4:
                    CheckTrip();
                    break;
                case 5:
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("Wrong entry");
                    break;
            }

        }

        private void CheckTrip()
        {
            foreach (var x in Trips.Include(x => x.Passengers))
                Console.WriteLine(
                    $@"id:{x.TripId} from {x.From} {x.Takeof} to {x.To} {x.Arrival}, {x.Passengers.Count}- passengers");
        }

        private void CheckPassenger()
        {
            foreach (var x in Passengers)
                Console.WriteLine(x.PassengerId + " " + x.Name);
        }

        private void CheckAirplane()
        {
            foreach (var x in Airplanes.Include(x => x.Company))
                Console.WriteLine($@"id:{x.AirplaneId},  name:{x.Manufacturer}  {x.Model},   country:{x.Company.Name}");
        }

        private void CheckCompany()
        {
            foreach (var x in Companies.Include(x => x.Airplanes))
                Console.WriteLine(
                    $@"id:{x.CompanyId},  name:{x.Name},   country:{x.Country}, planes: {x.Airplanes.Count} pcs.");
        }

        internal void FillDatabase()
        {
            Random rnd = new Random();
            //Passengers
            String[] names =
            {
                "Akhmed", "Said", "Ilias", "Ali", "Ivan",
                "Fjodor", "Veniamin", "Roy", "Brandon", "Roger",
                "Janze", "Shijenkun", "Shaolin", "Mirko", "Luca",
                "Slobodan", "Zukhra", "Giuli", "Zulikha", "Kimberly",
                "Sheron", "Samanta", "Evproksia", "Marina", "Vasilisa",
                "Syaomin", "Huowi", "Sanson"
            };
            for(int i=0;i<400;i++)AddPassenger(names[rnd.Next(0,names.Length-1)]);

            //Air companies

            Pair[] companies =
            {
                new Pair("Qatar", "Qatar"), new Pair("AirAsia", "Thailand"),
                new Pair("VietnamAirlines", "Vietnam"), new Pair("Aeroflot", "Russia"),
                new Pair("AirBaltic", "Latvia"), new Pair("FinAir", "Finland"),
                new Pair("AirFrance", "France")
            };
            foreach (var x in companies) AddCompany(x.company, x.country);

            //Airplanes
            
            for(int i=0; i<50; i++)
            {
                int m = rnd.Next(0,2);
                String[] manu = {"Boeing", "Airbus"};
                
                String[] boeings = {"747", "777", "737", "787"};
                String[] airbusses = { "310", "320", "321" };

                String model = m < 1 ? boeings[rnd.Next(0, 4)] : airbusses[rnd.Next(0, 3)];

                AddAirplane(manu[m],model, new DateTime (rnd.Next(1989,2015)), rnd.Next(1, Companies.Count()));
            }
            //Flights
            for (int i = 0; i < 10; i++)
            {
               
                DateTime takeof = GetRandomSummerTime();
                DateTime arrival = takeof.AddHours(11).AddMinutes(rnd.Next(15,57));
                AddTrip
                    (
                        "LED, Saint-Petersburg, Pulkovo",
                        "BKK, Bangkok, Suvarnabhumi", 
                        takeof, arrival,
                        rnd.Next(1,Airplanes.Count())
                    );
            }

            for (int i = 0; i < 3; i++)
            {

                DateTime takeof = GetRandomSummerTime();
                DateTime arrival = takeof.AddHours(3).AddMinutes(rnd.Next(15, 57));
                AddTrip("LED, Saint-Petersburg, Pulkovo", "MLA, Malta", takeof, arrival, rnd.Next(1, Airplanes.Count()));
            }
            for (int i = 0; i < 15; i++)
            {

                DateTime takeof = GetRandomSummerTime();
                DateTime arrival = takeof.AddHours(3).AddMinutes(rnd.Next(0, 20));
                AddTrip("LED, Saint-Petersburg, Pulkovo", "DRS, Drezden", takeof, arrival, rnd.Next(1, Airplanes.Count()));
            }
            for (int i = 0; i < 4; i++)
            {

                DateTime takeof = GetRandomSummerTime();
                DateTime arrival = takeof.AddHours(4).AddMinutes(rnd.Next(20, 40));
                AddTrip("LED, Saint-Petersburg, Pulkovo", "GYD, Geidar Aliev Airport", takeof, arrival, rnd.Next(1, Airplanes.Count()));
            }

            for (int i = 0; i < 20; i++)
            {

                DateTime takeof = GetRandomSummerTime();
                DateTime arrival = takeof.AddHours(4).AddMinutes(rnd.Next(5, 25));
                AddTrip("LED, Saint-Petersburg, Pulkovo", "PRG, Vatslahaveld, Prague", takeof, arrival, rnd.Next(1, Airplanes.Count()));
            }
            for (int i = 0; i < 9; i++)
            {

                DateTime takeof = GetRandomSummerTime();
                DateTime arrival = takeof.AddHours(5).AddMinutes(rnd.Next(20, 40));
                AddTrip("LED, Saint-Petersburg, Pulkovo", "LIS, Lisbon Portella Airport", takeof, arrival, rnd.Next(1, Airplanes.Count()));
            }

            //Lets sell tickets!!
           
            for (int i = 1; i < Passengers.Count(); i++)
            {
                Tickets.Add(new Ticket {PassengerId = i, TripId = rnd.Next(1, Trips.Count())});
            }


            SaveChanges();
        }

        DateTime GetRandomSummerTime()
        {
            Random rnd = new Random();
            int mo = rnd.Next(5, 10);
            int d = rnd.Next(1, 30);
            int h = rnd.Next(0, 23);
            int mi = rnd.Next(0, 59);
            return new DateTime(2020, mo, d, h, mi, 0);
        }

    }
}


internal class Pair
    {
        internal String country; internal String company;
        internal int num1; internal int num2;
        internal Pair(String a, String b) { company = a; country = b; }
        internal Pair(int a, int b) { num1 = a; num2 = b; }
}





