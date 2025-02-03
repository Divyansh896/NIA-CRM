using NIA_CRM.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace NIA_CRM.Data
{
    public static class NIACRMInitializer
    {
        /// <summary>
        /// Prepares the Database and seeds data as required
        /// </summary>
        /// <param name="serviceProvider">DI Container</param>
        /// <param name="DeleteDatabase">Delete the database and start from scratch</param>
        /// <param name="UseMigrations">Use Migrations or EnsureCreated</param>
        /// <param name="SeedSampleData">Add optional sample data</param>
        public static void Initialize(IServiceProvider serviceProvider,
     bool DeleteDatabase = false, bool UseMigrations = true, bool SeedSampleData = true)
        {
            using (var context = new NIACRMContext(
                serviceProvider.GetRequiredService<DbContextOptions<NIACRMContext>>()))
            {
                //Refresh the database as per the parameter options
                #region Prepare the Database
                try
                {
                    //Note: .CanConnect() will return false if the database is not there!
                    if (DeleteDatabase || !context.Database.CanConnect())
                    {
                        context.Database.EnsureDeleted(); //Delete the existing version 
                        if (UseMigrations)
                        {
                            context.Database.Migrate(); //Create the Database and apply all migrations
                        }
                        else
                        {
                            context.Database.EnsureCreated(); //Create and update the database as per the Model
                        }
                        //Now create any additional database objects such as Triggers or Views
                        //--------------------------------------------------------------------
                        //Create the Triggers
                        string sqlCmd = @"
                            CREATE TRIGGER SetProductionEmailTimestampOnUpdate
                            AFTER UPDATE ON ProductionEmail
                            BEGIN
                                UPDATE ProductionEmail
                                SET RowVersion = randomblob(8)
                                WHERE rowId = NEW.rowId;
                            END;
                        ";
                        context.Database.ExecuteSqlRaw(sqlCmd);

                        sqlCmd = @"
                            CREATE TRIGGER SetProductionEmailTimestampOnInsert
                            AFTER INSERT ON ProductionEmail
                            BEGIN
                                UPDATE ProductionEmail
                                SET RowVersion = randomblob(8)
                                WHERE rowId = NEW.rowId;
                            END
                        ";
                        context.Database.ExecuteSqlRaw(sqlCmd);
                    }
                    else //The database is already created
                    {
                        if (UseMigrations)
                        {
                            context.Database.Migrate(); //Apply all migrations
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.GetBaseException().Message);
                }

                /* try
                 {
                     //Note: .CanConnect() will return false if the database is not there!
                     if (DeleteDatabase || !context.Database.CanConnect())
                     {
                         context.Database.EnsureDeleted(); //Delete the existing version 
                         if (UseMigrations)
                         {
                             context.Database.Migrate(); //Create the Database and apply all migrations
                         }
                         else
                         {
                             context.Database.EnsureCreated(); //Create and update the database as per the Model
                         }
                     }

                     //Now create any additional database objects such as Triggers or Views
                     //--------------------------------------------------------------------

                     else //The database is already created
                     {
                         if (UseMigrations)
                         {
                             context.Database.Migrate(); //Apply all migrations
                         }
                     }

                 }
                 catch (Exception ex)
                 {
                     Debug.WriteLine(ex.GetBaseException().Message);
                 }*/

                try
                {
                    //Add some Class Start times

                    if (!context.MembershipTypes.Any())
                    {
                        context.MembershipTypes.AddRange(
                            new MembershipType
                            {
                                ID = 1,
                                TypeName = "Associate",
                                TypeDescr = "Access to gym equipment and locker room facilities."
                            },
                            new MembershipType
                            {
                                ID = 2,
                                TypeName = "Chamber,Associate",
                                TypeDescr = "Includes Basic Membership benefits plus access to group classes and pool."
                            },
                            new MembershipType
                            {
                                ID = 3,
                                TypeName = "Government & Education,Associate",
                                TypeDescr = "Includes Premium Membership benefits for up to 4 family members."
                            },
                            new MembershipType
                            {
                                ID = 4,
                                TypeName = "Local Industrial",
                                TypeDescr = "Discounted membership for students with valId Id."
                            },
                            new MembershipType
                            {
                                ID = 5,
                                TypeName = "Non-Local Industrial",
                                TypeDescr = "Special membership for employees of partner organizations."
                            }
                        );
                        context.SaveChanges();
                    }

                    if (!context.Members.Any())
                    {
                        context.Members.AddRange(
                           new Member
                           {
                               ID = 1,
                               MemberName = "John Doe",
                               MemberSize = 10,
                               JoinDate = new DateTime(2021, 1, 1),
                               WebsiteUrl = "https://www.johndoe.com",
                               Addresses = new List<Address>
                               {
                            new Address
                            {
                                AddressLine1 = "123 Main St",
                                AddressLine2 = "Apt 1B",
                                City = "New York",
                                StateProvince = "NY",
                                PostalCode = "10001",
                                Country = "USA"
                            }
                                               }
                           },
                                           new Member
                                           {
                                               ID = 2,
                                               MemberName = "Jane Smith",
                                               MemberSize = 5,
                                               JoinDate = new DateTime(2020, 6, 15),
                                               WebsiteUrl = "https://www.janesmith.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "456 Oak Ave",
                                AddressLine2 = "Unit 2A",
                                City = "Los Angeles",
                                StateProvince = "CA",
                                PostalCode = "90001",
                                Country = "USA"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 3,
                                               MemberName = "Emily Johnson",
                                               MemberSize = 8,
                                               JoinDate = new DateTime(2019, 4, 21),
                                               WebsiteUrl = "https://www.emilyjohnson.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "789 Pine Rd",
                                AddressLine2 = "Suite 3C",
                                City = "Chicago",
                                StateProvince = "IL",
                                PostalCode = "60601",
                                Country = "USA"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 4,
                                               MemberName = "Michael Brown",
                                               MemberSize = 6,
                                               JoinDate = new DateTime(2021, 7, 11),
                                               WebsiteUrl = "https://www.michaelbrown.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "101 Maple St",
                                AddressLine2 = "Apt 4D",
                                City = "San Francisco",
                                StateProvince = "CA",
                                PostalCode = "94101",
                                Country = "USA"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 5,
                                               MemberName = "Sarah Davis",
                                               MemberSize = 4,
                                               JoinDate = new DateTime(2020, 3, 25),
                                               WebsiteUrl = "https://www.sarahdavis.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "555 Birch Blvd",
                                AddressLine2 = "Unit 7B",
                                City = "Houston",
                                StateProvince = "TX",
                                PostalCode = "77001",
                                Country = "USA"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 6,
                                               MemberName = "David Martinez",
                                               MemberSize = 12,
                                               JoinDate = new DateTime(2022, 5, 19),
                                               WebsiteUrl = "https://www.davidmartinez.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "888 Cedar St",
                                AddressLine2 = "Apt 10E",
                                City = "Austin",
                                StateProvince = "TX",
                                PostalCode = "73301",
                                Country = "USA"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 7,
                                               MemberName = "Robert Wilson",
                                               MemberSize = 15,
                                               JoinDate = new DateTime(2018, 2, 7),
                                               WebsiteUrl = "https://www.robertwilson.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "222 Elm St",
                                AddressLine2 = "Suite 5A",
                                City = "Dallas",
                                StateProvince = "TX",
                                PostalCode = "75201",
                                Country = "USA"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 8,
                                               MemberName = "William Moore",
                                               MemberSize = 3,
                                               JoinDate = new DateTime(2020, 8, 30),
                                               WebsiteUrl = "https://www.williammoore.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "333 Ash Ave",
                                AddressLine2 = "Unit 2C",
                                City = "Phoenix",
                                StateProvince = "AZ",
                                PostalCode = "85001",
                                Country = "USA"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 9,
                                               MemberName = "Olivia Taylor",
                                               MemberSize = 9,
                                               JoinDate = new DateTime(2022, 10, 18),
                                               WebsiteUrl = "https://www.oliviataylor.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "444 Birch Rd",
                                AddressLine2 = "Suite 5B",
                                City = "Seattle",
                                StateProvince = "WA",
                                PostalCode = "98101",
                                Country = "USA"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 10,
                                               MemberName = "Sophia Anderson",
                                               MemberSize = 7,
                                               JoinDate = new DateTime(2019, 5, 21),
                                               WebsiteUrl = "https://www.sophiaanderson.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "555 Oak Blvd",
                                AddressLine2 = "Unit 1A",
                                City = "Denver",
                                StateProvince = "CO",
                                PostalCode = "80201",
                                Country = "USA"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 11,
                                               MemberName = "James Thomas",
                                               MemberSize = 2,
                                               JoinDate = new DateTime(2021, 12, 8),
                                               WebsiteUrl = "https://www.jamesthomas.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "111 Maple Rd",
                                AddressLine2 = "Apt 2C",
                                City = "Chicago",
                                StateProvince = "IL",
                                PostalCode = "60607",
                                Country = "USA"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 12,
                                               MemberName = "Daniel Lee",
                                               MemberSize = 11,
                                               JoinDate = new DateTime(2017, 11, 15),
                                               WebsiteUrl = "https://www.daniellee.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "333 Pine Blvd",
                                AddressLine2 = "Apt 1F",
                                City = "San Francisco",
                                StateProvince = "CA",
                                PostalCode = "94105",
                                Country = "USA"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 13,
                                               MemberName = "Lucas Harris",
                                               MemberSize = 8,
                                               JoinDate = new DateTime(2022, 9, 22),
                                               WebsiteUrl = "https://www.lucasharris.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "777 Oak Rd",
                                AddressLine2 = "Suite 5B",
                                City = "Miami",
                                StateProvince = "FL",
                                PostalCode = "33101",
                                Country = "USA"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 14,
                                               MemberName = "Ella Walker",
                                               MemberSize = 6,
                                               JoinDate = new DateTime(2022, 6, 30),
                                               WebsiteUrl = "https://www.ellawalker.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "555 Birch St",
                                AddressLine2 = "Unit 4A",
                                City = "New York",
                                StateProvince = "NY",
                                PostalCode = "10002",
                                Country = "USA"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 15,
                                               MemberName = "Liam Robinson",
                                               MemberSize = 4,
                                               JoinDate = new DateTime(2021, 3, 12),
                                               WebsiteUrl = "https://www.liamrobinson.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "200 Maple Rd",
                                AddressLine2 = "Unit 6D",
                                City = "Boston",
                                StateProvince = "MA",
                                PostalCode = "02110",
                                Country = "USA"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 16,
                                               MemberName = "Ava Lewis",
                                               MemberSize = 4,
                                               JoinDate = new DateTime(2021, 11, 12),
                                               WebsiteUrl = "https://www.avalewis.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "965 Elm St",
                                AddressLine2 = "Apt 7A",
                                City = "Quebec City",
                                StateProvince = "Quebec",
                                PostalCode = "G1K 4Y7",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 17,
                                               MemberName = "Ethan Walker",
                                               MemberSize = 3,
                                               JoinDate = new DateTime(2020, 4, 28),
                                               WebsiteUrl = "https://www.ethanwalker.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "124 Maple Blvd",
                                AddressLine2 = "Unit 6",
                                City = "Vancouver",
                                StateProvince = "British Columbia",
                                PostalCode = "V6J 3M2",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 18,
                                               MemberName = "Mason King",
                                               MemberSize = 6,
                                               JoinDate = new DateTime(2021, 10, 4),
                                               WebsiteUrl = "https://www.masonking.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "481 Cedar Ave",
                                AddressLine2 = "Apt 12B",
                                City = "Toronto",
                                StateProvince = "Ontario",
                                PostalCode = "M4E 5W1",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 19,
                                               MemberName = "Lucas Green",
                                               MemberSize = 4,
                                               JoinDate = new DateTime(2022, 4, 18),
                                               WebsiteUrl = "https://www.lucasgreen.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "187 Birch Rd",
                                AddressLine2 = "Suite 4",
                                City = "Ottawa",
                                StateProvince = "Ontario",
                                PostalCode = "K2P 1P1",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 20,
                                               MemberName = "Charlotte Hall",
                                               MemberSize = 9,
                                               JoinDate = new DateTime(2021, 7, 14),
                                               WebsiteUrl = "https://www.charlottehall.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "922 Cedar St",
                                AddressLine2 = "Unit 5A",
                                City = "Calgary",
                                StateProvince = "Alberta",
                                PostalCode = "T2N 1X5",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 21,
                                               MemberName = "Benjamin Harris",
                                               MemberSize = 7,
                                               JoinDate = new DateTime(2021, 8, 30),
                                               WebsiteUrl = "https://www.benjaminharris.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "643 Cedar Blvd",
                                AddressLine2 = "Apt 9D",
                                City = "Montreal",
                                StateProvince = "Quebec",
                                PostalCode = "H2Y 1G3",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 22,
                                               MemberName = "Aiden Clark",
                                               MemberSize = 2,
                                               JoinDate = new DateTime(2022, 9, 7),
                                               WebsiteUrl = "https://www.aidenclark.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "876 Maple Rd",
                                AddressLine2 = "Unit 1B",
                                City = "Ottawa",
                                StateProvince = "Ontario",
                                PostalCode = "K1A 0B1",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 23,
                                               MemberName = "Ella Moore",
                                               MemberSize = 3,
                                               JoinDate = new DateTime(2020, 12, 15),
                                               WebsiteUrl = "https://www.ellamoore.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "134 Pine St",
                                AddressLine2 = "Apt 6A",
                                City = "Toronto",
                                StateProvince = "Ontario",
                                PostalCode = "M5A 3J2",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 24,
                                               MemberName = "Jacob White",
                                               MemberSize = 5,
                                               JoinDate = new DateTime(2021, 5, 21),
                                               WebsiteUrl = "https://www.jacobwhite.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "789 Oak St",
                                AddressLine2 = "Suite 4B",
                                City = "Ottawa",
                                StateProvince = "Ontario",
                                PostalCode = "K2P 7J9",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 25,
                                               MemberName = "Abigail Nelson",
                                               MemberSize = 8,
                                               JoinDate = new DateTime(2020, 10, 18),
                                               WebsiteUrl = "https://www.abigailnelson.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "233 Cedar St",
                                AddressLine2 = "Unit 7C",
                                City = "Vancouver",
                                StateProvince = "British Columbia",
                                PostalCode = "V6Z 3A7",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 26,
                                               MemberName = "Mason Lee",
                                               MemberSize = 6,
                                               JoinDate = new DateTime(2021, 7, 14),
                                               WebsiteUrl = "https://www.masonlee.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "111 Birch Blvd",
                                AddressLine2 = "Apt 4D",
                                City = "Calgary",
                                StateProvince = "Alberta",
                                PostalCode = "T2P 5B8",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 27,
                                               MemberName = "Chloe Scott",
                                               MemberSize = 9,
                                               JoinDate = new DateTime(2022, 1, 22),
                                               WebsiteUrl = "https://www.chloescott.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "533 Cedar Rd",
                                AddressLine2 = "Unit 2B",
                                City = "Toronto",
                                StateProvince = "Ontario",
                                PostalCode = "M5G 3B9",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 28,
                                               MemberName = "Daniel Harris",
                                               MemberSize = 7,
                                               JoinDate = new DateTime(2022, 7, 11),
                                               WebsiteUrl = "https://www.danielharris.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "987 Maple St",
                                AddressLine2 = "Apt 3A",
                                City = "Ottawa",
                                StateProvince = "Ontario",
                                PostalCode = "K1R 4T2",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 29,
                                               MemberName = "Ava Carter",
                                               MemberSize = 4,
                                               JoinDate = new DateTime(2021, 9, 3),
                                               WebsiteUrl = "https://www.avacarter.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "444 Oak Blvd",
                                AddressLine2 = "Suite 8B",
                                City = "Vancouver",
                                StateProvince = "British Columbia",
                                PostalCode = "V5Y 2B6",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 30,
                                               MemberName = "Landon Walker",
                                               MemberSize = 5,
                                               JoinDate = new DateTime(2022, 8, 18),
                                               WebsiteUrl = "https://www.landonwalker.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "872 Cedar Rd",
                                AddressLine2 = "Apt 9C",
                                City = "Ottawa",
                                StateProvince = "Ontario",
                                PostalCode = "K2K 5B9",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 31,
                                               MemberName = "Amelia Harris",
                                               MemberSize = 6,
                                               JoinDate = new DateTime(2022, 6, 13),
                                               WebsiteUrl = "https://www.ameliaharris.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "1234 Oak Blvd",
                                AddressLine2 = "Apt 2C",
                                City = "Montreal",
                                StateProvince = "Quebec",
                                PostalCode = "H2X 1N8",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 32,
                                               MemberName = "Oliver Lee",
                                               MemberSize = 9,
                                               JoinDate = new DateTime(2022, 7, 6),
                                               WebsiteUrl = "https://www.oliverlee.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "990 Pine Rd",
                                AddressLine2 = "Unit 7",
                                City = "Calgary",
                                StateProvince = "Alberta",
                                PostalCode = "T2P 7N3",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 33,
                                               MemberName = "Harper Scott",
                                               MemberSize = 4,
                                               JoinDate = new DateTime(2021, 5, 10),
                                               WebsiteUrl = "https://www.harperscott.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "522 Cedar Rd",
                                AddressLine2 = "Suite 6A",
                                City = "Hamilton",
                                StateProvince = "Ontario",
                                PostalCode = "L8N 1A3",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 34,
                                               MemberName = "Sophie Adams",
                                               MemberSize = 7,
                                               JoinDate = new DateTime(2022, 1, 24),
                                               WebsiteUrl = "https://www.sophieadams.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "690 Birch St",
                                AddressLine2 = "Unit 3A",
                                City = "Toronto",
                                StateProvince = "Ontario",
                                PostalCode = "M4G 2B3",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 35,
                                               MemberName = "Isaac Morgan",
                                               MemberSize = 2,
                                               JoinDate = new DateTime(2021, 4, 9),
                                               WebsiteUrl = "https://www.isaacmorgan.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "123 Birch Rd",
                                AddressLine2 = "Apt 7C",
                                City = "Ottawa",
                                StateProvince = "Ontario",
                                PostalCode = "K1Y 2X5",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 36,
                                               MemberName = "Mia Thompson",
                                               MemberSize = 8,
                                               JoinDate = new DateTime(2022, 4, 19),
                                               WebsiteUrl = "https://www.miathompson.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "987 Birch Rd",
                                AddressLine2 = "Unit 4A",
                                City = "Vancouver",
                                StateProvince = "British Columbia",
                                PostalCode = "V6B 3E9",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 37,
                                               MemberName = "Ethan Johnson",
                                               MemberSize = 5,
                                               JoinDate = new DateTime(2022, 10, 10),
                                               WebsiteUrl = "https://www.ethanjohnson.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "456 Oak Rd",
                                AddressLine2 = "Suite 2B",
                                City = "Ottawa",
                                StateProvince = "Ontario",
                                PostalCode = "K2P 1V8",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 38,
                                               MemberName = "Grace Miller",
                                               MemberSize = 4,
                                               JoinDate = new DateTime(2022, 5, 15),
                                               WebsiteUrl = "https://www.gracemiller.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "890 Cedar Blvd",
                                AddressLine2 = "Suite 2B",
                                City = "Montreal",
                                StateProvince = "Quebec",
                                PostalCode = "H3B 2G4",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 39,
                                               MemberName = "Lily Turner",
                                               MemberSize = 3,
                                               JoinDate = new DateTime(2022, 8, 22),
                                               WebsiteUrl = "https://www.lilyturner.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "800 Maple Blvd",
                                AddressLine2 = "Unit 5A",
                                City = "Vancouver",
                                StateProvince = "British Columbia",
                                PostalCode = "V6B 1N4",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 40,
                                               MemberName = "Liam Walker",
                                               MemberSize = 6,
                                               JoinDate = new DateTime(2021, 3, 18),
                                               WebsiteUrl = "https://www.liamwalker.com",
                                               Addresses = new List<Address>
                                               {
                                                new Address
                                                {
                                                    AddressLine1 = "354 Cedar St",
                                                    AddressLine2 = "Apt 6D",
                                                    City = "Calgary",
                                                    StateProvince = "Alberta",
                                                    PostalCode = "T2V 1Z6",
                                                    Country = "Canada"
                                                }
                                                }

                                           }
                                           );


                        context.SaveChanges();
                    }
                    //if (!context.MemberIndustries.Any())
                    //{
                    //    context.MemberIndustries.AddRange(
                    //        new MemberIndustry { MemberId = 1, IndustryId = 1 },
                    //        new MemberIndustry { MemberId = 2, IndustryId = 2 },
                    //        new MemberIndustry { MemberId = 3, IndustryId = 3 },
                    //        new MemberIndustry { MemberId = 4, IndustryId = 4 },
                    //        new MemberIndustry { MemberId = 5, IndustryId = 5 },
                    //        new MemberIndustry { MemberId = 6, IndustryId = 6 },
                    //        new MemberIndustry { MemberId = 7, IndustryId = 7 },
                    //        new MemberIndustry { MemberId = 8, IndustryId = 8 },
                    //        new MemberIndustry { MemberId = 9, IndustryId = 9 },
                    //        new MemberIndustry { MemberId = 10, IndustryId = 10 },
                    //        new MemberIndustry { MemberId = 10, IndustryId = 41 },
                    //        new MemberIndustry { MemberId = 11, IndustryId = 11 },
                    //        new MemberIndustry { MemberId = 12, IndustryId = 12 },
                    //        new MemberIndustry { MemberId = 13, IndustryId = 13 },
                    //        new MemberIndustry { MemberId = 14, IndustryId = 14 },
                    //        new MemberIndustry { MemberId = 15, IndustryId = 15 },
                    //        new MemberIndustry { MemberId = 15, IndustryId = 42 },
                    //        new MemberIndustry { MemberId = 16, IndustryId = 16 },
                    //        new MemberIndustry { MemberId = 17, IndustryId = 17 },
                    //        new MemberIndustry { MemberId = 18, IndustryId = 18 },
                    //        new MemberIndustry { MemberId = 19, IndustryId = 19 },
                    //        new MemberIndustry { MemberId = 20, IndustryId = 20 },
                    //        new MemberIndustry { MemberId = 21, IndustryId = 21 },
                    //        new MemberIndustry { MemberId = 22, IndustryId = 22 },
                    //        new MemberIndustry { MemberId = 23, IndustryId = 23 },
                    //        new MemberIndustry { MemberId = 24, IndustryId = 24 },
                    //        new MemberIndustry { MemberId = 25, IndustryId = 25 },
                    //        new MemberIndustry { MemberId = 25, IndustryId = 43 },
                    //        new MemberIndustry { MemberId = 26, IndustryId = 26 },
                    //        new MemberIndustry { MemberId = 27, IndustryId = 27 },
                    //        new MemberIndustry { MemberId = 28, IndustryId = 28 },
                    //        new MemberIndustry { MemberId = 29, IndustryId = 29 },
                    //        new MemberIndustry { MemberId = 30, IndustryId = 30 },
                    //        new MemberIndustry { MemberId = 31, IndustryId = 31 },
                    //        new MemberIndustry { MemberId = 32, IndustryId = 32 },
                    //        new MemberIndustry { MemberId = 32, IndustryId = 44 },
                    //        new MemberIndustry { MemberId = 33, IndustryId = 33 },
                    //        new MemberIndustry { MemberId = 34, IndustryId = 34 },
                    //        new MemberIndustry { MemberId = 35, IndustryId = 35 },
                    //        new MemberIndustry { MemberId = 36, IndustryId = 36 },
                    //        new MemberIndustry { MemberId = 37, IndustryId = 37 },
                    //        new MemberIndustry { MemberId = 38, IndustryId = 38 },
                    //        new MemberIndustry { MemberId = 39, IndustryId = 39 },
                    //        new MemberIndustry { MemberId = 40, IndustryId = 40 },
                    //        new MemberIndustry { MemberId = 40, IndustryId = 45 }
                    //        );

                    //    context.SaveChanges();
                    //}
                    if (!context.MemberMembershipTypes.Any())
                    {
                        context.MemberMembershipTypes.AddRange(
                            new MemberMembershipType
                            {
                                MemberId = 1, // John Doe
                                MembershipTypeId = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 1, // John Doe
                                MembershipTypeId = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 2, // Jane Smith
                                MembershipTypeId = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 3, // Robert Johnson
                                MembershipTypeId = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 4, // Emily Davis
                                MembershipTypeId = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 5, // William Brown
                                MembershipTypeId = 2 // Silver Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 6, // Olivia Clark
                                MembershipTypeId = 5 // Platinum Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 7, // Noah Miller
                                MembershipTypeId = 3 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 8, // Sophia Wilson
                                MembershipTypeId = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 9, // Liam Martinez
                                MembershipTypeId = 4 // Gold Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 10, // Ava Anderson
                                MembershipTypeId = 2 // Silver Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 11, // Mason Lee
                                MembershipTypeId = 5 //
                            },
                            new MemberMembershipType
                            {
                                MemberId = 12,
                                MembershipTypeId = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 13,
                                MembershipTypeId = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 14,
                                MembershipTypeId = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 15,
                                MembershipTypeId = 5 // Corporate Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 16,
                                MembershipTypeId = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 17,
                                MembershipTypeId = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 18,
                                MembershipTypeId = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 19,
                                MembershipTypeId = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 20,
                                MembershipTypeId = 5 // Corporate Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 21,
                                MembershipTypeId = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 22,
                                MembershipTypeId = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 23,
                                MembershipTypeId = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 24,
                                MembershipTypeId = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 25,
                                MembershipTypeId = 5 // Corporate Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 26,
                                MembershipTypeId = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 27,
                                MembershipTypeId = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 28,
                                MembershipTypeId = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 29,
                                MembershipTypeId = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 30,
                                MembershipTypeId = 5 // Corporate Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 31,
                                MembershipTypeId = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 32,
                                MembershipTypeId = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 33,
                                MembershipTypeId = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 34,
                                MembershipTypeId = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 35,
                                MembershipTypeId = 5 // Corporate Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 36,
                                MembershipTypeId = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 37,
                                MembershipTypeId = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 38,
                                MembershipTypeId = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 39,
                                MembershipTypeId = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 40,
                                MembershipTypeId = 5 // Corporate Membership
                            }
                        );
                        context.SaveChanges();
                    }
                    if (!context.Contacts.Any())
                    {
                        context.Contacts.AddRange(
                            new Contact
                            {
                                Id = 1,
                                FirstName = "John",
                                LastName = "Thomas",
                                Title = "Manager",
                                Department = "Sales",
                                Email = "john.doe@example.com",
                                Phone = "1234567890",
                                LinkedInUrl = "https://www.linkedin.com/in/johndoe",
                                IsVip = true,
                                MemberId = 1
                            },
                            new Contact
                            {
                                Id = 2,
                                FirstName = "Jane",
                                LastName = "Smith",
                                Title = "Director",
                                Department = "Marketing",
                                Email = "jane.smith@example.com",
                                Phone = "9876543210",
                                LinkedInUrl = "https://www.linkedin.com/in/janesmith",
                                IsVip = false,
                                MemberId = 2
                            },
                            new Contact
                            {
                                Id = 3,
                                FirstName = "Alice",
                                LastName = "Johnson",
                                Title = "VP",
                                Department = "Human Resources",
                                Email = "alice.johnson@example.com",
                                Phone = "5551234567",
                                LinkedInUrl = "https://www.linkedin.com/in/alicejohnson",
                                IsVip = true,
                                MemberId = 3
                            },
                            new Contact
                            {
                                Id = 4,
                                FirstName = "Bob",
                                LastName = "Joe",
                                Title = "Chief Financial Officer",
                                Department = "Finance",
                                Email = "bob.brown@example.com",
                                Phone = "5557654321",
                                LinkedInUrl = "https://www.linkedin.com/in/bobbrown",
                                IsVip = true,
                                MemberId = 4
                            },
                            new Contact
                            {
                                Id = 5,
                                FirstName = "Charlie",
                                LastName = "Davis",
                                Title = "Chief Operating Officer",
                                Department = "Operations",
                                Email = "charlie.davis@example.com",
                                Phone = "5557890123",
                                LinkedInUrl = "https://www.linkedin.com/in/charliedavis",
                                IsVip = false,
                                MemberId = 5
                            },
                            new Contact
                            {
                                Id = 6,
                                FirstName = "Deborah",
                                LastName = "Williams",
                                Title = "Director of Technology",
                                Department = "Technology",
                                Email = "deborah.williams@example.com",
                                Phone = "5552345678",
                                LinkedInUrl = "https://www.linkedin.com/in/deborahwilliams",
                                IsVip = true,
                                MemberId = 6
                            },
                            new Contact
                            {
                                Id = 7,
                                FirstName = "Eve",
                                LastName = "Marie",
                                Title = "Marketing Specialist",
                                Department = "Marketing",
                                Email = "eve.taylor@example.com",
                                Phone = "5553456789",
                                LinkedInUrl = "https://www.linkedin.com/in/evetaylor",
                                IsVip = false,
                                MemberId = 7
                            },
                            new Contact
                            {
                                Id = 8,
                                FirstName = "Frank",
                                LastName = "Harris",
                                Title = "Senior Engineer",
                                Department = "Engineering",
                                Email = "frank.harris@example.com",
                                Phone = "5554567890",
                                LinkedInUrl = "https://www.linkedin.com/in/frankharris",
                                IsVip = true,
                                MemberId = 8
                            },
                            new Contact
                            {
                                Id = 9,
                                FirstName = "Grace",
                                LastName = "King",
                                Title = "Business Development Manager",
                                Department = "Sales",
                                Email = "grace.king@example.com",
                                Phone = "5555678901",
                                LinkedInUrl = "https://www.linkedin.com/in/graceking",
                                IsVip = false,
                                MemberId = 9
                            },
                            new Contact
                            {
                                Id = 10,
                                FirstName = "Hank",
                                LastName = "Lee",
                                Title = "Head of Research",
                                Department = "Research and Development",
                                Email = "hank.lee@example.com",
                                Phone = "5556789012",
                                LinkedInUrl = "https://www.linkedin.com/in/hanklee",
                                IsVip = true,
                                MemberId = 10
                            },
                            new Contact
                            {
                                Id = 11,
                                FirstName = "Ivy",
                                LastName = "Adams",
                                Title = "Project Manager",
                                Department = "Operations",
                                Email = "ivy.adams@example.com",
                                Phone = "5557890123",
                                LinkedInUrl = "https://www.linkedin.com/in/ivyadams",
                                IsVip = false,
                                MemberId = 11
                            },
                            new Contact
                            {
                                Id = 12,
                                FirstName = "Jack",
                                LastName = "Scott",
                                Title = "CEO",
                                Department = "Executive",
                                Email = "jack.scott@example.com",
                                Phone = "5558901234",
                                LinkedInUrl = "https://www.linkedin.com/in/jackscott",
                                IsVip = true,
                                MemberId = 12
                            },
                            new Contact
                            {
                                Id = 13,
                                FirstName = "Kathy",
                                LastName = "Elizabeth",
                                Title = "HR Specialist",
                                Department = "Human Resources",
                                Email = "kathy.morris@example.com",
                                Phone = "5559012345",
                                LinkedInUrl = "https://www.linkedin.com/in/kathymorris",
                                IsVip = false,
                                MemberId = 13
                            },
                            new Contact
                            {
                                Id = 14,
                                FirstName = "Louis",
                                LastName = "Alexandr",
                                Title = "Customer Service Lead",
                                Department = "Customer Service",
                                Email = "louis.walker@example.com",
                                Phone = "5550123456",
                                LinkedInUrl = "https://www.linkedin.com/in/louiswalker",
                                IsVip = true,
                                MemberId = 14
                            },
                            new Contact
                            {
                                Id = 15,
                                FirstName = "Mona",
                                LastName = "Grace",
                                Title = "Legal Advisor",
                                Department = "Legal",
                                Email = "mona.white@example.com",
                                Phone = "5551234567",
                                LinkedInUrl = "https://www.linkedin.com/in/monawhite",
                                IsVip = false,
                                MemberId = 15
                            },
                            new Contact
                            {
                                Id = 16,
                                FirstName = "James",
                                LastName = "T.",
                                Title = "Marketing Manager",
                                Department = "Marketing",
                                Email = "james.smith@example.com",
                                Phone = "5559876543",
                                LinkedInUrl = "https://www.linkedin.com/in/jamessmith",
                                IsVip = false,
                                MemberId = 16
                            },
                            new Contact
                            {
                                Id = 17,
                                FirstName = "Sarah",
                                LastName = "A.",
                                Title = "HR Specialist",
                                Department = "Human Resources",
                                Email = "sarah.johnson@example.com",
                                Phone = "5551122334",
                                LinkedInUrl = "https://www.linkedin.com/in/sarahjohnson",
                                IsVip = false,
                                MemberId = 17
                            },
                            new Contact
                            {
                                Id = 18,
                                FirstName = "DavId",
                                LastName = "L.",
                                Title = "Chief Executive Officer",
                                Department = "Executive",
                                Email = "davId.brown@example.com",
                                Phone = "5552233445",
                                LinkedInUrl = "https://www.linkedin.com/in/davIdbrown",
                                IsVip = true,
                                MemberId = 18
                            },
                            new Contact
                            {
                                Id = 19,
                                FirstName = "Emily",
                                LastName = "M.",
                                Title = "Product Designer",
                                Department = "Design",
                                Email = "emily.williams@example.com",
                                Phone = "5556677889",
                                LinkedInUrl = "https://www.linkedin.com/in/emilywilliams",
                                IsVip = false,
                                MemberId = 19
                            },
                            new Contact
                            {
                                Id = 20,
                                FirstName = "Michael",
                                LastName = "J.",
                                Title = "Sales Director",
                                Department = "Sales",
                                Email = "michael.davis@example.com",
                                Phone = "5558899001",
                                LinkedInUrl = "https://www.linkedin.com/in/michaeldavis",
                                IsVip = false,
                                MemberId = 20
                            },
                            new Contact
                            {
                                Id = 21,
                                FirstName = "Olivia",
                                LastName = "K.",
                                Title = "Chief Financial Officer",
                                Department = "Finance",
                                Email = "olivia.martinez@example.com",
                                Phone = "5553456789",
                                LinkedInUrl = "https://www.linkedin.com/in/oliviamartinez",
                                IsVip = true,
                                MemberId = 21
                            },
                            new Contact
                            {
                                Id = 22,
                                FirstName = "Ethan",
                                LastName = "B.",
                                Title = "IT Manager",
                                Department = "IT",
                                Email = "ethan.taylor@example.com",
                                Phone = "5552345678",
                                LinkedInUrl = "https://www.linkedin.com/in/ethantaylor",
                                IsVip = false,
                                MemberId = 22
                            },
                            new Contact
                            {
                                Id = 23,
                                FirstName = "Sophia",
                                LastName = "J.",
                                Title = "Operations Coordinator",
                                Department = "Operations",
                                Email = "sophia.wilson@example.com",
                                Phone = "5556781234",
                                LinkedInUrl = "https://www.linkedin.com/in/sophiawilson",
                                IsVip = false,
                                MemberId = 23
                            },
                            new Contact
                            {
                                Id = 24,
                                FirstName = "Daniel",
                                LastName = "P.",
                                Title = "Customer Success Manager",
                                Department = "Customer Support",
                                Email = "daniel.moore@example.com",
                                Phone = "5559988776",
                                LinkedInUrl = "https://www.linkedin.com/in/danielmoore",
                                IsVip = false,
                                MemberId = 24
                            },
                        new Contact
                        {
                            Id = 25,
                            FirstName = "Chloe",
                            LastName = "S.",
                            Title = "Senior Analyst",
                            Department = "Finance",
                            Email = "chloe.martin@example.com",
                            Phone = "5557766554",
                            LinkedInUrl = "https://www.linkedin.com/in/chloemartin",
                            IsVip = true,
                            MemberId = 25
                        },
                        new Contact
                        {
                            Id = 26,
                            FirstName = "Liam",
                            LastName = "G.",
                            Title = "Project Manager",
                            Department = "Operations",
                            Email = "liam.green@example.com",
                            Phone = "5554433221",
                            LinkedInUrl = "https://www.linkedin.com/in/liamgreen",
                            IsVip = false,
                            MemberId = 26
                        },
                        new Contact
                        {
                            Id = 27,
                            FirstName = "Isabella",
                            LastName = "H.",
                            Title = "Marketing Director",
                            Department = "Marketing",
                            Email = "isabella.hudson@example.com",
                            Phone = "5559988776",
                            LinkedInUrl = "https://www.linkedin.com/in/isabellahudson",
                            IsVip = true,
                            MemberId = 27
                        },
                        new Contact
                        {
                            Id = 28,
                            FirstName = "Ethan",
                            LastName = "P.",
                            Title = "Sales Manager",
                            Department = "Sales",
                            Email = "ethan.peters@example.com",
                            Phone = "5551122334",
                            LinkedInUrl = "https://www.linkedin.com/in/ethanpeters",
                            IsVip = false,
                            MemberId = 28
                        },
                        new Contact
                        {
                            Id = 29,
                            FirstName = "Ava",
                            LastName = "W.",
                            Title = "Human Resources Specialist",
                            Department = "HR",
                            Email = "ava.williams@example.com",
                            Phone = "5556677889",
                            LinkedInUrl = "https://www.linkedin.com/in/avawilliams",
                            IsVip = true,
                            MemberId = 29
                        },
                        new Contact
                        {
                            Id = 30,
                            FirstName = "Mason",
                            LastName = "J.",
                            Title = "Data Scientist",
                            Department = "IT",
                            Email = "mason.james@example.com",
                            Phone = "5554455667",
                            LinkedInUrl = "https://www.linkedin.com/in/masonjames",
                            IsVip = false,
                            MemberId = 30
                        },
                        new Contact
                        {
                            Id = 31,
                            FirstName = "Sophia",
                            LastName = "K.",
                            Title = "Customer Support Lead",
                            Department = "Customer Service",
                            Email = "sophia.king@example.com",
                            Phone = "5552334455",
                            LinkedInUrl = "https://www.linkedin.com/in/sophiaking",
                            IsVip = true,
                            MemberId = 31
                        },
                        new Contact
                        {
                            Id = 32,
                            FirstName = "Jackson",
                            LastName = "T.",
                            Title = "Chief Technology Officer",
                            Department = "Technology",
                            Email = "jackson.taylor@example.com",
                            Phone = "5555555555",
                            LinkedInUrl = "https://www.linkedin.com/in/jacksontaylor",
                            IsVip = true,
                            MemberId = 32
                        },
                        new Contact
                        {
                            Id = 33,
                            FirstName = "Charlotte",
                            LastName = "L.",
                            Title = "Finance Manager",
                            Department = "Finance",
                            Email = "charlotte.larson@example.com",
                            Phone = "5552233446",
                            LinkedInUrl = "https://www.linkedin.com/in/charlottelarson",
                            IsVip = false,
                            MemberId = 33
                        },
                        new Contact
                        {
                            Id = 34,
                            FirstName = "Lucas",
                            LastName = "B.",
                            Title = "IT Specialist",
                            Department = "IT",
                            Email = "lucas.brown@example.com",
                            Phone = "5556677880",
                            LinkedInUrl = "https://www.linkedin.com/in/lucasbrown",
                            IsVip = true,
                            MemberId = 34
                        },
                        new Contact
                        {
                            Id = 35,
                            FirstName = "Mia",
                            LastName = "C.",
                            Title = "Legal Advisor",
                            Department = "Legal",
                            Email = "mia.carter@example.com",
                            Phone = "5553334446",
                            LinkedInUrl = "https://www.linkedin.com/in/miacarter",
                            IsVip = true,
                            MemberId = 35
                        },
                        new Contact
                        {
                            Id = 36,
                            FirstName = "Logan",
                            LastName = "D.",
                            Title = "Operations Manager",
                            Department = "Operations",
                            Email = "logan.davis@example.com",
                            Phone = "5553344556",
                            LinkedInUrl = "https://www.linkedin.com/in/logandavis",
                            IsVip = false,
                            MemberId = 36
                        },
                        new Contact
                        {
                            Id = 37,
                            FirstName = "Harper",
                            LastName = "M.",
                            Title = "Product Manager",
                            Department = "Product",
                            Email = "harper.morris@example.com",
                            Phone = "5555566778",
                            LinkedInUrl = "https://www.linkedin.com/in/harpermorris",
                            IsVip = true,
                            MemberId = 37
                        },
                        new Contact
                        {
                            Id = 38,
                            FirstName = "Benjamin",
                            LastName = "N.",
                            Title = "Chief Executive Officer",
                            Department = "Executive",
                            Email = "benjamin.nelson@example.com",
                            Phone = "5551223344",
                            LinkedInUrl = "https://www.linkedin.com/in/benjaminnelson",
                            IsVip = true,
                            MemberId = 38
                        },
                        new Contact
                        {
                            Id = 39,
                            FirstName = "Ella",
                            LastName = "O.",
                            Title = "Public Relations Manager",
                            Department = "PR",
                            Email = "ella.olson@example.com",
                            Phone = "5554455668",
                            LinkedInUrl = "https://www.linkedin.com/in/ellaolson",
                            IsVip = false,
                            MemberId = 39
                        },
                        new Contact
                        {
                            Id = 40,
                            FirstName = "James",
                            LastName = "P.",
                            Title = "Chief Marketing Officer",
                            Department = "Marketing",
                            Email = "james.phillips@example.com",
                            Phone = "5557768999",
                            LinkedInUrl = "https://www.linkedin.com/in/jamesphillips",
                            IsVip = true,
                            MemberId = 40
                        }

                        );
                        context.SaveChanges();
                    }


                    if (!context.Cancellations.Any())
                    {
                        context.Cancellations.AddRange(
                            // 2 Canceled records
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 5),
                                Canceled = true,
                                CancellationNote = "Member requested cancellation due to personal reasons.",
                                MemberID = 1  // Assuming Member with Id 1 exists
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 15),
                                Canceled = true,
                                CancellationNote = "Member canceled their subscription after failing to make payments.",
                                MemberID = 2  // Assuming Member with Id 2 exists
                            },

                            // 13 Non-Canceled records
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 10),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 3
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 12),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 4
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 18),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 5
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 20),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 6
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 22),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 7
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 25),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 8
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 27),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 9
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 30),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 10
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 2, 1),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 11
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 2, 5),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 12
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 2, 10),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 13
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 2, 15),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 14
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 2, 18),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 15
                            }
                        );
                        context.SaveChanges();
                    }
                    //Seed data needed for production and during development

                    if (!context.NAICSCodes.Any())
                    {
                        context.NAICSCodes.AddRange(
                            new NAICSCode { Id = 1, Label = "Agriculture", Code = "1111", Description = "Oilseed and Grain Farming" },
                            new NAICSCode { Id = 2, Label = "Mining", Code = "2111", Description = "Oil and Gas Extraction" },
                            new NAICSCode { Id = 3, Label = "Utilities", Code = "2211", Description = "Electric Power Generation, Transmission, and Distribution" },
                            new NAICSCode { Id = 4, Label = "Construction", Code = "2361", Description = "Residential Building Construction" },
                            new NAICSCode { Id = 5, Label = "Manufacturing", Code = "3111", Description = "Animal Food Manufacturing" },
                            new NAICSCode { Id = 6, Label = "Wholesale Trade", Code = "4231", Description = "Motor Vehicle and Motor Vehicle Parts and Supplies Wholesalers" },
                            new NAICSCode { Id = 7, Label = "Retail Trade", Code = "4411", Description = "Automobile Dealers" },
                            new NAICSCode { Id = 8, Label = "Transportation", Code = "4811", Description = "Scheduled Air Transportation" },
                            new NAICSCode { Id = 9, Label = "Information", Code = "5111", Description = "Newspaper, Periodical, Book, and Directory Publishers" },
                            new NAICSCode { Id = 10, Label = "Finance", Code = "5221", Description = "Depository Credit Intermediation" },
                            new NAICSCode { Id = 11, Label = "Real Estate", Code = "5311", Description = "Lessors of Real Estate" },
                            new NAICSCode { Id = 12, Label = "Professional Services", Code = "5411", Description = "Legal Services" },
                            new NAICSCode { Id = 13, Label = "Administrative Support", Code = "5611", Description = "Office Administrative Services" },
                            new NAICSCode { Id = 14, Label = "Education", Code = "6111", Description = "Elementary and Secondary Schools" },
                            new NAICSCode { Id = 15, Label = "Health Care", Code = "6211", Description = "Offices of Physicians" },
                            new NAICSCode { Id = 16, Label = "Arts & Entertainment", Code = "7111", Description = "Performing Arts Companies" },
                            new NAICSCode { Id = 17, Label = "Accommodation", Code = "7211", Description = "Traveler Accommodation" },
                            new NAICSCode { Id = 18, Label = "Automotive", Code = "8111", Description = "Automotive Repair and Maintenance" },
                            new NAICSCode { Id = 19, Label = "Public Administration", Code = "9211", Description = "Executive Offices" }
                        );

                        // Save changes to persist the data
                        context.SaveChanges();
                    }

                    if (!context.IndustryNAICSCodes.Any())
                    {
                        context.IndustryNAICSCodes.AddRange(
                            new IndustryNAICSCode { Id = 1, MemberId = 1, NAICSCodeId = 1 },  // Member 1, NAICS Code 1
                            new IndustryNAICSCode { Id = 2, MemberId = 2, NAICSCodeId = 2 },  // Member 2, NAICS Code 2
                            new IndustryNAICSCode { Id = 3, MemberId = 3, NAICSCodeId = 3 },  // Member 3, NAICS Code 3
                            new IndustryNAICSCode { Id = 4, MemberId = 4, NAICSCodeId = 4 },  // Member 4, NAICS Code 4
                            new IndustryNAICSCode { Id = 5, MemberId = 5, NAICSCodeId = 5 },  // Member 5, NAICS Code 5
                            new IndustryNAICSCode { Id = 6, MemberId = 6, NAICSCodeId = 6 },  // Member 6, NAICS Code 6
                            new IndustryNAICSCode { Id = 7, MemberId = 7, NAICSCodeId = 7 },  // Member 7, NAICS Code 7
                            new IndustryNAICSCode { Id = 8, MemberId = 8, NAICSCodeId = 8 },  // Member 8, NAICS Code 8
                            new IndustryNAICSCode { Id = 9, MemberId = 9, NAICSCodeId = 9 },  // Member 9, NAICS Code 9
                            new IndustryNAICSCode { Id = 10, MemberId = 10, NAICSCodeId = 10 },  // Member 10, NAICS Code 10
                            new IndustryNAICSCode { Id = 11, MemberId = 11, NAICSCodeId = 11 },  // Member 11, NAICS Code 11
                            new IndustryNAICSCode { Id = 12, MemberId = 12, NAICSCodeId = 12 },  // Member 12, NAICS Code 12
                            new IndustryNAICSCode { Id = 13, MemberId = 13, NAICSCodeId = 13 },  // Member 13, NAICS Code 13
                            new IndustryNAICSCode { Id = 14, MemberId = 14, NAICSCodeId = 14 },  // Member 14, NAICS Code 14
                            new IndustryNAICSCode { Id = 15, MemberId = 15, NAICSCodeId = 15 },  // Member 15, NAICS Code 15
                            new IndustryNAICSCode { Id = 16, MemberId = 16, NAICSCodeId = 16 },  // Member 16, NAICS Code 16
                            new IndustryNAICSCode { Id = 17, MemberId = 17, NAICSCodeId = 17 },  // Member 17, NAICS Code 17
                            new IndustryNAICSCode { Id = 18, MemberId = 18, NAICSCodeId = 18 },  // Member 18, NAICS Code 18
                            new IndustryNAICSCode { Id = 19, MemberId = 19, NAICSCodeId = 19 },  // Member 19, NAICS Code 19
                            new IndustryNAICSCode { Id = 20, MemberId = 20, NAICSCodeId = 1 },  // Member 20, NAICS Code 1
                            new IndustryNAICSCode { Id = 21, MemberId = 21, NAICSCodeId = 2 },  // Member 21, NAICS Code 2
                            new IndustryNAICSCode { Id = 22, MemberId = 22, NAICSCodeId = 3 },  // Member 22, NAICS Code 3
                            new IndustryNAICSCode { Id = 23, MemberId = 23, NAICSCodeId = 4 },  // Member 23, NAICS Code 4
                            new IndustryNAICSCode { Id = 24, MemberId = 24, NAICSCodeId = 5 },  // Member 24, NAICS Code 5
                            new IndustryNAICSCode { Id = 25, MemberId = 25, NAICSCodeId = 6 },  // Member 25, NAICS Code 6
                            new IndustryNAICSCode { Id = 26, MemberId = 26, NAICSCodeId = 7 },  // Member 26, NAICS Code 7
                            new IndustryNAICSCode { Id = 27, MemberId = 27, NAICSCodeId = 8 },  // Member 27, NAICS Code 8
                            new IndustryNAICSCode { Id = 28, MemberId = 28, NAICSCodeId = 9 },  // Member 28, NAICS Code 9
                            new IndustryNAICSCode { Id = 29, MemberId = 29, NAICSCodeId = 10 },  // Member 29, NAICS Code 10
                            new IndustryNAICSCode { Id = 30, MemberId = 30, NAICSCodeId = 11 },  // Member 30, NAICS Code 11
                            new IndustryNAICSCode { Id = 31, MemberId = 31, NAICSCodeId = 12 },  // Member 31, NAICS Code 12
                            new IndustryNAICSCode { Id = 32, MemberId = 32, NAICSCodeId = 13 },  // Member 32, NAICS Code 13
                            new IndustryNAICSCode { Id = 33, MemberId = 33, NAICSCodeId = 14 },  // Member 33, NAICS Code 14
                            new IndustryNAICSCode { Id = 34, MemberId = 34, NAICSCodeId = 15 },  // Member 34, NAICS Code 15
                            new IndustryNAICSCode { Id = 35, MemberId = 35, NAICSCodeId = 16 },  // Member 35, NAICS Code 16
                            new IndustryNAICSCode { Id = 36, MemberId = 36, NAICSCodeId = 17 },  // Member 36, NAICS Code 17
                            new IndustryNAICSCode { Id = 37, MemberId = 37, NAICSCodeId = 18 },  // Member 37, NAICS Code 18
                            new IndustryNAICSCode { Id = 38, MemberId = 38, NAICSCodeId = 19 },  // Member 38, NAICS Code 19
                            new IndustryNAICSCode { Id = 39, MemberId = 39, NAICSCodeId = 1 },  // Member 39, NAICS Code 1
                            new IndustryNAICSCode { Id = 40, MemberId = 40, NAICSCodeId = 2 }   // Member 40, NAICS Code 2
                        );

                        // Save changes to persist the data
                        context.SaveChanges();
                    }


                    if (!context.ProductionEmails.Any())
                    {
                        // Random member names to be used in the Email content
                        var randomNames = new List<string>
                            {
                                "John Doe", "Jane Smith", "Robert Johnson", "Emily Davis", "Michael Brown"
                            };

                        // Seeding predefined production Email templates with random member names
                        context.ProductionEmails.AddRange(
                             new ProductionEmail
                             {
                                 Id = 1,
                                 EmailType = "Welcome Email",
                                 Subject = "Welcome to NIA!",
                                 Body = $"Dear {randomNames[0]},\n\nWelcome to the NIA community! We are thrilled to have you onboard. Please let us know if you need any assistance.\n\nBest regards,\nNIA Team",
                                 //IsGood = true
                             },
                             new ProductionEmail
                             {
                                 Id = 2,
                                 EmailType = "Renewal Reminder",
                                 Subject = "Membership Renewal Reminder",
                                 Body = $"Dear {randomNames[1]},\n\nYour membership with NIA is about to expire on 2025-02-15. Please renew your membership to continue enjoying all the benefits.\n\nBest regards,\nNIA Team",
                                 //IsGood = true
                             },
                             new ProductionEmail
                             {
                                 Id = 3,
                                 EmailType = "Cancellation Notice",
                                 Subject = "Membership Cancellation Confirmation",
                                 Body = $"Dear {randomNames[2]},\n\nWe are sorry to see you go. Your membership has been successfully canceled. If you change your mind in the future, we’d love to have you back.\n\nBest regards,\nNIA Team",
                                 //IsGood = true
                             },
                             new ProductionEmail
                             {
                                 Id = 4,
                                 EmailType = "Membership Update",
                                 Subject = "Important Membership Update",
                                 Body = $"Dear {randomNames[3]},\n\nWe would like to inform you of an important update regarding your membership status. Please log in to your account for more details.\n\nBest regards,\nNIA Team",
                                 //IsGood = true
                             },
                             new ProductionEmail
                             {
                                 Id = 5,
                                 EmailType = "General Notification",
                                 Subject = "NIA System Update",
                                 Body = $"Dear {randomNames[4]},\n\nThis is a notification regarding a recent update to the NIA system. We encourage you to check out the new features and improvements.\n\nBest regards,\nNIA Team",
                                 //IsGood = true
                             });
                        context.SaveChanges();
                    }

                    if (!context.MemberNotes.Any())
                    {
                        context.MemberNotes.AddRange(
                        new MemberNote { Id = 1, MemberId = 1, Note = "Initial onboarding discussion with the client about the new CRM implementation project.", CreatedAt = DateTime.Now.AddDays(-30) },
                        new MemberNote { Id = 2, MemberId = 2, Note = "Client follow-up meeting to discuss the status of the CRM project.", CreatedAt = DateTime.Now.AddDays(-28) },
                        new MemberNote { Id = 3, MemberId = 3, Note = "Reviewed the feedback provided by the client regarding the CRM customization.", CreatedAt = DateTime.Now.AddDays(-25) },
                        new MemberNote { Id = 4, MemberId = 4, Note = "Final meeting to present the completed CRM project and gather client feedback.", CreatedAt = DateTime.Now.AddDays(-20) },
                        new MemberNote { Id = 5, MemberId = 5, Note = "Discussed potential upgrades to the CRM system and next steps for implementation.", CreatedAt = DateTime.Now.AddDays(-15) },
                        new MemberNote { Id = 6, MemberId = 6, Note = "Follow-up meeting on CRM feedback and next implementation steps.", CreatedAt = DateTime.Now.AddDays(-14) },
                        new MemberNote { Id = 7, MemberId = 7, Note = "Presentation of CRM reports and how they impact client operations.", CreatedAt = DateTime.Now.AddDays(-13) },
                        new MemberNote { Id = 8, MemberId = 8, Note = "Discussed additional CRM modules with the client.", CreatedAt = DateTime.Now.AddDays(-12) },
                        new MemberNote { Id = 9, MemberId = 9, Note = "Client requested further training on CRM features.", CreatedAt = DateTime.Now.AddDays(-10) },
                        new MemberNote { Id = 10, MemberId = 10, Note = "Assessed CRM performance and discussed any issues with the client.", CreatedAt = DateTime.Now.AddDays(-9) },
                        new MemberNote { Id = 11, MemberId = 11, Note = "Discussed CRM data integration challenges with client.", CreatedAt = DateTime.Now.AddDays(-8) },
                        new MemberNote { Id = 12, MemberId = 12, Note = "Client praised CRM's data accuracy and reporting tools.", CreatedAt = DateTime.Now.AddDays(-7) },
                        new MemberNote { Id = 13, MemberId = 13, Note = "Discussed CRM security enhancements and data privacy concerns.", CreatedAt = DateTime.Now.AddDays(-6) },
                        new MemberNote { Id = 14, MemberId = 14, Note = "Review of CRM system uptime and performance metrics.", CreatedAt = DateTime.Now.AddDays(-5) },
                        new MemberNote { Id = 15, MemberId = 15, Note = "Final meeting to present CRM system's success and discuss potential upgrades.", CreatedAt = DateTime.Now.AddDays(-4) },
                        new MemberNote { Id = 16, MemberId = 16, Note = "Client follow-up on CRM project post-implementation.", CreatedAt = DateTime.Now.AddDays(-3) },
                        new MemberNote { Id = 17, MemberId = 17, Note = "CRM update meeting scheduled for next month.", CreatedAt = DateTime.Now.AddDays(-2) },
                        new MemberNote { Id = 18, MemberId = 18, Note = "Client expressed satisfaction with CRM customization.", CreatedAt = DateTime.Now.AddDays(-1) },
                        new MemberNote { Id = 19, MemberId = 19, Note = "Discussed the CRM's future roadmap with the client.", CreatedAt = DateTime.Now.AddDays(-30) },
                        new MemberNote { Id = 20, MemberId = 20, Note = "Explained the new CRM reporting features to the client.", CreatedAt = DateTime.Now.AddDays(-29) },
                        new MemberNote { Id = 21, MemberId = 21, Note = "Meeting to review CRM's feature requests and client requirements.", CreatedAt = DateTime.Now.AddDays(-28) },
                        new MemberNote { Id = 22, MemberId = 22, Note = "CRM performance review meeting with the client team.", CreatedAt = DateTime.Now.AddDays(-27) },
                        new MemberNote { Id = 23, MemberId = 23, Note = "Discussions on improving CRM speed and scalability.", CreatedAt = DateTime.Now.AddDays(-26) },
                        new MemberNote { Id = 24, MemberId = 24, Note = "Client feedback session on CRM user interface improvements.", CreatedAt = DateTime.Now.AddDays(-25) },
                        new MemberNote { Id = 25, MemberId = 25, Note = "Client to provide feedback on the newly integrated CRM modules.", CreatedAt = DateTime.Now.AddDays(-24) },
                        new MemberNote { Id = 26, MemberId = 26, Note = "Final training session on CRM features for the client team.", CreatedAt = DateTime.Now.AddDays(-23) },
                        new MemberNote { Id = 27, MemberId = 27, Note = "Discussed potential issues with CRM's third-party integration.", CreatedAt = DateTime.Now.AddDays(-22) },
                        new MemberNote { Id = 28, MemberId = 28, Note = "Client inquired about additional CRM customization options.", CreatedAt = DateTime.Now.AddDays(-21) },
                        new MemberNote { Id = 29, MemberId = 29, Note = "Debrief on CRM deployment and initial reactions from the client team.", CreatedAt = DateTime.Now.AddDays(-20) },
                        new MemberNote { Id = 30, MemberId = 30, Note = "Scheduled CRM review call for next week with the client.", CreatedAt = DateTime.Now.AddDays(-19) },
                        new MemberNote { Id = 31, MemberId = 31, Note = "Client requested demo on advanced CRM reporting capabilities.", CreatedAt = DateTime.Now.AddDays(-18) },
                        new MemberNote { Id = 32, MemberId = 32, Note = "Client shared feedback on CRM's mobile interface and app.", CreatedAt = DateTime.Now.AddDays(-17) },
                        new MemberNote { Id = 33, MemberId = 33, Note = "Discussed client satisfaction and future CRM upgrades.", CreatedAt = DateTime.Now.AddDays(-16) },
                        new MemberNote { Id = 34, MemberId = 34, Note = "Followed up with the client regarding CRM bug fixes.", CreatedAt = DateTime.Now.AddDays(-15) },
                        new MemberNote { Id = 35, MemberId = 35, Note = "Revisited CRM's customization options for the client.", CreatedAt = DateTime.Now.AddDays(-14) },
                        new MemberNote { Id = 36, MemberId = 36, Note = "Review meeting with the client about CRM's current status.", CreatedAt = DateTime.Now.AddDays(-13) },
                        new MemberNote { Id = 37, MemberId = 37, Note = "Client requested documentation for CRM integration with existing systems.", CreatedAt = DateTime.Now.AddDays(-12) },
                        new MemberNote { Id = 38, MemberId = 38, Note = "Explained CRM's data storage architecture to the client.", CreatedAt = DateTime.Now.AddDays(-11) },
                        new MemberNote { Id = 39, MemberId = 39, Note = "Client follow-up regarding CRM's email marketing tools.", CreatedAt = DateTime.Now.AddDays(-10) },
                        new MemberNote { Id = 40, MemberId = 40, Note = "Discussed CRM's scalability and future-proofing with the client.", CreatedAt = DateTime.Now.AddDays(-9) }
                        );

                        context.SaveChanges();

                    }
                }



                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);

                }


                #endregion
            }

        }


    }
}
