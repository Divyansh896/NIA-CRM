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
                               MemberName = "Alpha Steel",
                               MemberSize = 10,
                               JoinDate = new DateTime(2021, 1, 1),
                               WebsiteUrl = "https://www.johndoe.com",
                               Addresses = new List<Address>
                               {
                            new Address
                            {
                                AddressLine1 = "123 Main St",
                                AddressLine2 = "Apt 1B",
                                City = "Niagara Falls",
                                StateProvince = "ON",
                                PostalCode = "L2G 3Y7",
                                Country = "Canada"
                            }
                                               }
                           },
                                           new Member
                                           {
                                               ID = 2,
                                               MemberName = "TISCO CO.",
                                               MemberSize = 5,
                                               JoinDate = new DateTime(2020, 6, 15),
                                               WebsiteUrl = "https://www.janesmith.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "456 Oak Ave",
                                AddressLine2 = "Unit 2A",
                                City = "Niagara Falls",
                                StateProvince = "ON",
                                PostalCode = "L2H 1H4",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 3,
                                               MemberName = "M Time Irons",
                                               MemberSize = 8,
                                               JoinDate = new DateTime(2019, 4, 21),
                                               WebsiteUrl = "https://www.emilyjohnson.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "789 Pine Rd",
                                AddressLine2 = "Suite 3C",
                                City = "Niagara Falls",
                                StateProvince = "ON",
                                PostalCode = "L2E 6S5",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 4,
                                               MemberName = "Forge & Foundry Inc.",
                                               MemberSize = 6,
                                               JoinDate = new DateTime(2021, 7, 11),
                                               WebsiteUrl = "https://www.michaelbrown.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "101 Maple St",
                                AddressLine2 = "Apt 4D",
                                City = "Niagara Falls",
                                StateProvince = "ON",
                                PostalCode = "L2E 1B1",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 5,
                                               MemberName = "Northern Metalworks",
                                               MemberSize = 4,
                                               JoinDate = new DateTime(2020, 3, 25),
                                               WebsiteUrl = "https://www.sarahdavis.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "555 Birch Blvd",
                                AddressLine2 = "Unit 7B",
                                City = "Niagara Falls",
                                StateProvince = "ON",
                                PostalCode = "L2G 7M7",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 6,
                                               MemberName = "Titanium Solutions",
                                               MemberSize = 12,
                                               JoinDate = new DateTime(2022, 5, 19),
                                               WebsiteUrl = "https://www.davidmartinez.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "888 Cedar St",
                                AddressLine2 = "Apt 10E",
                                City = "St. Catharines",
                                StateProvince = "ON",
                                PostalCode = "L2M 3Y3",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 7,
                                               MemberName = "Phoenix Alloys",
                                               MemberSize = 15,
                                               JoinDate = new DateTime(2018, 2, 7),
                                               WebsiteUrl = "https://www.robertwilson.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "222 Elm St",
                                AddressLine2 = "Suite 5A",
                                City = "St. Catharines",
                                StateProvince = "ON",
                                PostalCode = "L2P 3H2",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 8,
                                               MemberName = "Galaxy Metals",
                                               MemberSize = 3,
                                               JoinDate = new DateTime(2020, 8, 30),
                                               WebsiteUrl = "https://www.williammoore.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "333 Ash Ave",
                                AddressLine2 = "Unit 2C",
                                City = "St. Catharines",
                                StateProvince = "ON",
                                PostalCode = "L2N 5V4",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 9,
                                               MemberName = "Ironclad Industries",
                                               MemberSize = 9,
                                               JoinDate = new DateTime(2022, 10, 18),
                                               WebsiteUrl = "https://www.oliviataylor.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "444 Birch Rd",
                                AddressLine2 = "Suite 5B",
                                City = "St. Catharines",
                                StateProvince = "ON",
                                PostalCode = "L2T 2P3",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 10,
                                               MemberName = "Silverline Fabrication",
                                               MemberSize = 7,
                                               JoinDate = new DateTime(2019, 5, 21),
                                               WebsiteUrl = "https://www.sophiaanderson.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "555 Oak Blvd",
                                AddressLine2 = "Unit 1A",
                                City = "St. Catharines",
                                StateProvince = "ON",
                                PostalCode = "L2S 1P9",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 11,
                                               MemberName = "Star Steelworks",
                                               MemberSize = 2,
                                               JoinDate = new DateTime(2021, 12, 8),
                                               WebsiteUrl = "https://www.jamesthomas.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "111 Maple Rd",
                                AddressLine2 = "Apt 2C",
                                City = "Welland",
                                StateProvince = "ON",
                                PostalCode = "L3B 1A1",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 12,
                                               MemberName = "Summit Metal Co.",
                                               MemberSize = 11,
                                               JoinDate = new DateTime(2017, 11, 15),
                                               WebsiteUrl = "https://www.daniellee.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "333 Pine Blvd",
                                AddressLine2 = "Apt 1F",
                                City = "Welland",
                                StateProvince = "ON",
                                PostalCode = "L3B 5N9",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 13,
                                               MemberName = "Everest Iron Corp.",
                                               MemberSize = 8,
                                               JoinDate = new DateTime(2022, 9, 22),
                                               WebsiteUrl = "https://www.lucasharris.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "777 Oak Rd",
                                AddressLine2 = "Suite 5B",
                                City = "Welland",
                                StateProvince = "ON",
                                PostalCode = "L3C 7C1",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 14,
                                               MemberName = "Prime Alloy Coatings",
                                               MemberSize = 6,
                                               JoinDate = new DateTime(2022, 6, 30),
                                               WebsiteUrl = "https://www.ellawalker.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "555 Birch St",
                                AddressLine2 = "Unit 4A",
                                City = "Welland",
                                StateProvince = "ON",
                                PostalCode = "L3C 4T6",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 15,
                                               MemberName = "Magnum Steel Solutions",
                                               MemberSize = 4,
                                               JoinDate = new DateTime(2021, 3, 12),
                                               WebsiteUrl = "https://www.liamrobinson.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "200 Maple Rd",
                                AddressLine2 = "Unit 6D",
                                City = "Welland",
                                StateProvince = "ON",
                                PostalCode = "L3C 2A9",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 16,
                                               MemberName = "Quantum Tech Innovations",
                                               MemberSize = 4,
                                               JoinDate = new DateTime(2021, 11, 12),
                                               WebsiteUrl = "https://www.avalewis.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "965 Elm St",
                                AddressLine2 = "Apt 7A",
                                City = "Thorold",
                                StateProvince = "ON",
                                PostalCode = "L2V 4Y6",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 17,
                                               MemberName = "Aurora Renewable Energy",
                                               MemberSize = 3,
                                               JoinDate = new DateTime(2020, 4, 28),
                                               WebsiteUrl = "https://www.ethanwalker.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "124 Maple Blvd",
                                AddressLine2 = "Unit 6",
                                City = "Thorold",
                                StateProvince = "ON",
                                PostalCode = "L2V 1H1",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 18,
                                               MemberName = "Vertex Financial Group",
                                               MemberSize = 6,
                                               JoinDate = new DateTime(2021, 10, 4),
                                               WebsiteUrl = "https://www.masonking.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "481 Cedar Ave",
                                AddressLine2 = "Apt 12B",
                                City = "Thorold",
                                StateProvince = "ON",
                                PostalCode = "L2V 3P2",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 19,
                                               MemberName = "Nova Biotech Labs",
                                               MemberSize = 4,
                                               JoinDate = new DateTime(2022, 4, 18),
                                               WebsiteUrl = "https://www.lucasgreen.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "187 Birch Rd",
                                AddressLine2 = "Suite 4",
                                City = "Thorold",
                                StateProvince = "ON",
                                PostalCode = "L2V 5Z8",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 20,
                                               MemberName = "Summit Construction Co.",
                                               MemberSize = 9,
                                               JoinDate = new DateTime(2021, 7, 14),
                                               WebsiteUrl = "https://www.charlottehall.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "922 Cedar St",
                                AddressLine2 = "Unit 5A",
                                City = "Thorold",
                                StateProvince = "ON",
                                PostalCode = "L2V 4K9",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 21,
                                               MemberName = "Oceanic Shipping Corp",
                                               MemberSize = 7,
                                               JoinDate = new DateTime(2021, 8, 30),
                                               WebsiteUrl = "https://www.benjaminharris.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "643 Cedar Blvd",
                                AddressLine2 = "Apt 9D",
                                City = "Port Colborne",
                                StateProvince = "ON",
                                PostalCode = "L3K 2W9",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 22,
                                               MemberName = "Evergreen Agriculture",
                                               MemberSize = 2,
                                               JoinDate = new DateTime(2022, 9, 7),
                                               WebsiteUrl = "https://www.aidenclark.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "876 Maple Rd",
                                AddressLine2 = "Unit 1B",
                                City = "Port Colborne",
                                StateProvince = "ON",
                                PostalCode = "L3K 3V2",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 23,
                                               MemberName = "Ironclad Manufacturing Ltd.",
                                               MemberSize = 3,
                                               JoinDate = new DateTime(2020, 12, 15),
                                               WebsiteUrl = "https://www.ellamoore.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "134 Pine St",
                                AddressLine2 = "Apt 6A",
                                City = "Port Colborne",
                                StateProvince = "ON",
                                PostalCode = "L3K 6A9",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 24,
                                               MemberName = "Skyline Architects Inc.",
                                               MemberSize = 5,
                                               JoinDate = new DateTime(2021, 5, 21),
                                               WebsiteUrl = "https://www.jacobwhite.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "789 Oak St",
                                AddressLine2 = "Suite 4B",
                                City = "Port Colborne",
                                StateProvince = "ON",
                                PostalCode = "L3K 5E8",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 25,
                                               MemberName = "Pinnacle Consulting Services",
                                               MemberSize = 8,
                                               JoinDate = new DateTime(2020, 10, 18),
                                               WebsiteUrl = "https://www.abigailnelson.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "233 Cedar St",
                                AddressLine2 = "Unit 7C",
                                City = "Port Colborne",
                                StateProvince = "ON",
                                PostalCode = "L3K 7X5",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 26,
                                               MemberName = "Crystal Water Solutions",
                                               MemberSize = 6,
                                               JoinDate = new DateTime(2021, 7, 14),
                                               WebsiteUrl = "https://www.masonlee.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "111 Birch Blvd",
                                AddressLine2 = "Apt 4D",
                                City = "Grimsby",
                                StateProvince = "ON",
                                PostalCode = "L3M 1R2",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 27,
                                               MemberName = "Elite Healthcare Partners",
                                               MemberSize = 9,
                                               JoinDate = new DateTime(2022, 1, 22),
                                               WebsiteUrl = "https://www.chloescott.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "533 Cedar Rd",
                                AddressLine2 = "Unit 2B",
                                City = "Grimsby",
                                StateProvince = "ON",
                                PostalCode = "L3M 4N6",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 28,
                                               MemberName = "Galaxy IT Solutions",
                                               MemberSize = 7,
                                               JoinDate = new DateTime(2022, 7, 11),
                                               WebsiteUrl = "https://www.danielharris.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "987 Maple St",
                                AddressLine2 = "Apt 3A",
                                City = "Grimsby",
                                StateProvince = "ON",
                                PostalCode = "L3M 3J5",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 29,
                                               MemberName = "Urban Infrastructure Group",
                                               MemberSize = 4,
                                               JoinDate = new DateTime(2021, 9, 3),
                                               WebsiteUrl = "https://www.avacarter.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "444 Oak Blvd",
                                AddressLine2 = "Suite 8B",
                                 City = "Grimsby",
                                StateProvince = "ON",
                                PostalCode = "L3M 2A8",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 30,
                                               MemberName = "Horizon Aerospace Inc.",
                                               MemberSize = 5,
                                               JoinDate = new DateTime(2022, 8, 18),
                                               WebsiteUrl = "https://www.landonwalker.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "872 Cedar Rd",
                                AddressLine2 = "Apt 9C",
                                City = "Grimsby",
                                StateProvince = "ON",
                                PostalCode = "L3M 5K9",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 31,
                                               MemberName = "Cobalt Mining Ventures",
                                               MemberSize = 6,
                                               JoinDate = new DateTime(2022, 6, 13),
                                               WebsiteUrl = "https://www.ameliaharris.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "1234 Oak Blvd",
                                AddressLine2 = "Apt 2C",
                                City = "Fort Erie",
                                StateProvince = "ON",
                                PostalCode = "L2A 5R1",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 32,
                                               MemberName = "Lakeside Resorts and Hotels",
                                               MemberSize = 9,
                                               JoinDate = new DateTime(2022, 7, 6),
                                               WebsiteUrl = "https://www.oliverlee.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "990 Pine Rd",
                                AddressLine2 = "Unit 7",
                                City = "Fort Erie",
                                StateProvince = "ON",
                                PostalCode = "L2A 7B9",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 33,
                                               MemberName = "NextGen Media Productions",
                                               MemberSize = 4,
                                               JoinDate = new DateTime(2021, 5, 10),
                                               WebsiteUrl = "https://www.harperscott.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "522 Cedar Rd",
                                AddressLine2 = "Suite 6A",
                                  City = "Fort Erie",
                                StateProvince = "ON",
                                PostalCode = "L2A 2T6",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 34,
                                               MemberName = "Crestwood Pharmaceutical",
                                               MemberSize = 7,
                                               JoinDate = new DateTime(2022, 1, 24),
                                               WebsiteUrl = "https://www.sophieadams.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "690 Birch St",
                                AddressLine2 = "Unit 3A",
                                City = "Fort Erie",
                                StateProvince = "ON",
                                PostalCode = "L2A 9W8",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 35,
                                               MemberName = "Dynamic Logistics Group",
                                               MemberSize = 2,
                                               JoinDate = new DateTime(2021, 4, 9),
                                               WebsiteUrl = "https://www.isaacmorgan.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "123 Birch Rd",
                                AddressLine2 = "Apt 7C",
                                City = "Fort Erie",
                                StateProvince = "ON",
                                PostalCode = "L2A 4K3",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 36,
                                               MemberName = "Northern Timber Products",
                                               MemberSize = 8,
                                               JoinDate = new DateTime(2022, 4, 19),
                                               WebsiteUrl = "https://www.miathompson.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "987 Birch Rd",
                                AddressLine2 = "Unit 4A",
                                City = "Lincoln",
                                StateProvince = "ON",
                                PostalCode = "L0R 1B1",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 37,
                                               MemberName = "Brightline Education Systems",
                                               MemberSize = 5,
                                               JoinDate = new DateTime(2022, 10, 10),
                                               WebsiteUrl = "https://www.ethanjohnson.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "456 Oak Rd",
                                AddressLine2 = "Suite 2B",
                                City = "Lincoln",
                                StateProvince = "ON",
                                PostalCode = "L0R 2C0",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 38,
                                               MemberName = "Fusion Energy Solutions",
                                               MemberSize = 4,
                                               JoinDate = new DateTime(2022, 5, 15),
                                               WebsiteUrl = "https://www.gracemiller.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "890 Cedar Blvd",
                                AddressLine2 = "Suite 2B",
                                City = "Pelham",
                                StateProvince = "ON",
                                PostalCode = "L0S 1C0",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 39,
                                               MemberName = "Trailblazer Automotive Group",
                                               MemberSize = 3,
                                               JoinDate = new DateTime(2022, 8, 22),
                                               WebsiteUrl = "https://www.lilyturner.com",
                                               Addresses = new List<Address>
                                               {
                            new Address
                            {
                                AddressLine1 = "800 Maple Blvd",
                                AddressLine2 = "Unit 5A",
                                City = "Pelham",
                                StateProvince = "ON",
                                PostalCode = "L0S 1E0",
                                Country = "Canada"
                            }
                                               }
                                           },
                                           new Member
                                           {
                                               ID = 40,
                                               MemberName = "Harvest Foods International",
                                               MemberSize = 6,
                                               JoinDate = new DateTime(2021, 3, 18),
                                               WebsiteUrl = "https://www.liamwalker.com",
                                               Addresses = new List<Address>
                                               {
                                                new Address
                                                {
                                                    AddressLine1 = "354 Cedar St",
                                                    AddressLine2 = "Apt 6D",
                                                    City = "Fort Erie",
                                                    StateProvince = "ON",
                                                    PostalCode = "L2A 1M7",
                                                    Country = "Canada"
                                                }
                                                }

                                           },
                                           new Member
                                           {
                                               ID = 41,
                                               MemberName = "Niagara Energy Solutions",
                                               MemberSize = 10,
                                               JoinDate = new DateTime(2020, 9, 12),
                                               WebsiteUrl = "https://www.jacobpeterson.com",
                                               Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "123 Power Ave",
                                                    AddressLine2 = "Suite 1B",
                                                    City = "Niagara Falls",
                                                    StateProvince = "ON",
                                                    PostalCode = "L2E 3P2",
                                                    Country = "Canada"
                                                }
                                            }
                                           },
                                        new Member
                                        {
                                            ID = 42,
                                            MemberName = "Brock University",
                                            MemberSize = 25,
                                            JoinDate = new DateTime(2021, 5, 3),
                                            WebsiteUrl = "https://www.marcusjones.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "500 Glenridge Ave",
                                                    AddressLine2 = "Building C",
                                                    City = "St. Catharines",
                                                    StateProvince = "ON",
                                                    PostalCode = "L2S 3A1",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 43,
                                            MemberName = "Niagara Healthcare Inc.",
                                            MemberSize = 15,
                                            JoinDate = new DateTime(2022, 1, 20),
                                            WebsiteUrl = "https://www.sarahmartin.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "45 Welland Ave",
                                                    AddressLine2 = "Unit 7B",
                                                    City = "Welland",
                                                    StateProvince = "ON",
                                                    PostalCode = "L3C 1V8",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 44,
                                            MemberName = "Niagara Financial Advisors",
                                            MemberSize = 20,
                                            JoinDate = new DateTime(2021, 8, 14),
                                            WebsiteUrl = "https://www.oliviamartinez.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "2500 South Service Rd",
                                                    AddressLine2 = "Suite 11A",
                                                    City = "Grimsby",
                                                    StateProvince = "ON",
                                                    PostalCode = "L3M 2R7",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 45,
                                            MemberName = "Vineyard Estates Winery",
                                            MemberSize = 30,
                                            JoinDate = new DateTime(2022, 4, 8),
                                            WebsiteUrl = "https://www.tylermorris.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "1234 Vine St",
                                                    AddressLine2 = "Winery Rd",
                                                    City = "Niagara-on-the-Lake",
                                                    StateProvince = "ON",
                                                    PostalCode = "L0S 1J0",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 46,
                                            MemberName = "St. Catharines Brewing Co.",
                                            MemberSize = 8,
                                            JoinDate = new DateTime(2020, 11, 2),
                                            WebsiteUrl = "https://www.danielcollins.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "29 Queen St",
                                                    AddressLine2 = "Brewery Lane",
                                                    City = "St. Catharines",
                                                    StateProvince = "ON",
                                                    PostalCode = "L2R 5A9",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 47,
                                            MemberName = "Niagara Logistics Solutions",
                                            MemberSize = 12,
                                            JoinDate = new DateTime(2021, 10, 10),
                                            WebsiteUrl = "https://www.rachelharris.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "55 Industrial Dr",
                                                    AddressLine2 = "Unit 3",
                                                    City = "Thorold",
                                                    StateProvince = "ON",
                                                    PostalCode = "L2V 2P9",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 48,
                                            MemberName = "Niagara Roofing & Construction",
                                            MemberSize = 10,
                                            JoinDate = new DateTime(2022, 2, 11),
                                            WebsiteUrl = "https://www.jamieclark.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "2141 Mewburn Rd",
                                                    AddressLine2 = "Suite 10",
                                                    City = "Niagara Falls",
                                                    StateProvince = "ON",
                                                    PostalCode = "L2G 7V6",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 49,
                                            MemberName = "The Port Colborne Bakery",
                                            MemberSize = 5,
                                            JoinDate = new DateTime(2021, 6, 17),
                                            WebsiteUrl = "https://www.nicholasanderson.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "1500 Elm St",
                                                    AddressLine2 = "Unit 4",
                                                    City = "Port Colborne",
                                                    StateProvince = "ON",
                                                    PostalCode = "L3K 5Y5",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 50,
                                            MemberName = "Sunset Motors",
                                            MemberSize = 7,
                                            JoinDate = new DateTime(2022, 5, 19),
                                            WebsiteUrl = "https://www.elizabethlee.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "123 Sunset Blvd",
                                                    AddressLine2 = "Car Sales",
                                                    City = "Niagara Falls",
                                                    StateProvince = "ON",
                                                    PostalCode = "L2E 6X5",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 51,
                                            MemberName = "Niagara Recycling Ltd.",
                                            MemberSize = 22,
                                            JoinDate = new DateTime(2021, 12, 25),
                                            WebsiteUrl = "https://www.nicholasjones.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "7893 South Niagara Pkwy",
                                                    AddressLine2 = "Recycling Plant",
                                                    City = "Niagara Falls",
                                                    StateProvince = "ON",
                                                    PostalCode = "L2E 6V8",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 52,
                                            MemberName = "Rosewood Estates Winery",
                                            MemberSize = 20,
                                            JoinDate = new DateTime(2022, 9, 18),
                                            WebsiteUrl = "https://www.meganvaughn.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "123 Rosewood Ave",
                                                    AddressLine2 = "Winery Rd",
                                                    City = "Niagara-on-the-Lake",
                                                    StateProvince = "ON",
                                                    PostalCode = "L0S 1J1",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 53,
                                            MemberName = "Niagara Falls Convention Centre",
                                            MemberSize = 18,
                                            JoinDate = new DateTime(2020, 7, 10),
                                            WebsiteUrl = "https://www.andrewjohnson.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "6815 Stanley Ave",
                                                    AddressLine2 = "Convention Centre",
                                                    City = "Niagara Falls",
                                                    StateProvince = "ON",
                                                    PostalCode = "L2G 3Y9",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 54,
                                            MemberName = "Niagara Home Furnishings",
                                            MemberSize = 14,
                                            JoinDate = new DateTime(2021, 4, 25),
                                            WebsiteUrl = "https://www.joshuasmith.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "7600 Montrose Rd",
                                                    AddressLine2 = "Furniture Store",
                                                    City = "Niagara Falls",
                                                    StateProvince = "ON",
                                                    PostalCode = "L2H 2T7",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 55,
                                            MemberName = "Niagara Construction Ltd.",
                                            MemberSize = 12,
                                            JoinDate = new DateTime(2020, 10, 14),
                                            WebsiteUrl = "https://www.justinwhite.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "1550 Ontario St",
                                                    AddressLine2 = "Unit 9",
                                                    City = "St. Catharines",
                                                    StateProvince = "ON",
                                                    PostalCode = "L2N 7Y4",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 56,
                                            MemberName = "Brockway Plumbing Services",
                                            MemberSize = 6,
                                            JoinDate = new DateTime(2021, 2, 7),
                                            WebsiteUrl = "https://www.jennifermartin.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "9800 Lundy's Lane",
                                                    AddressLine2 = "Unit 12",
                                                    City = "Niagara Falls",
                                                    StateProvince = "ON",
                                                    PostalCode = "L2H 1H7",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 57,
                                            MemberName = "Grand Niagara Golf Club",
                                            MemberSize = 25,
                                            JoinDate = new DateTime(2021, 11, 16),
                                            WebsiteUrl = "https://www.kimberlydavis.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "1500 Montrose Rd",
                                                    AddressLine2 = "Golf Club",
                                                    City = "Niagara Falls",
                                                    StateProvince = "ON",
                                                    PostalCode = "L2H 3N6",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 58,
                                            MemberName = "Niagara Peninsula Foods",
                                            MemberSize = 28,
                                            JoinDate = new DateTime(2020, 12, 5),
                                            WebsiteUrl = "https://www.ryanscott.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "6347 Stanley Ave",
                                                    AddressLine2 = "Unit 20",
                                                    City = "Niagara Falls",
                                                    StateProvince = "ON",
                                                    PostalCode = "L2G 3Z6",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 59,
                                            MemberName = "Niagara Falls Hospitality Group",
                                            MemberSize = 30,
                                            JoinDate = new DateTime(2021, 5, 10),
                                            WebsiteUrl = "https://www.dylanross.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "123 Victoria Ave",
                                                    AddressLine2 = "Hospitality Suite",
                                                    City = "Niagara Falls",
                                                    StateProvince = "ON",
                                                    PostalCode = "L2E 4Y3",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 60,
                                            MemberName = "Peachland Grocers",
                                            MemberSize = 14,
                                            JoinDate = new DateTime(2022, 3, 29),
                                            WebsiteUrl = "https://www.johnadams.com",
                                            Addresses = new List<Address>
                                            {
                                                new Address
                                                {
                                                    AddressLine1 = "8255 Greenhill Ave",
                                                    AddressLine2 = "Grocery Store",
                                                    City = "Pelham",
                                                    StateProvince = "ON",
                                                    PostalCode = "L0S 1E2",
                                                    Country = "Canada"
                                                }
                                            }
                                        },
                                        new Member
                                        {
                                            ID = 61,
                                            MemberName = "Niagara Valley Distillery",
                                            MemberSize = 8,
                                            JoinDate = new DateTime(2022, 8, 30),
                                            WebsiteUrl = "https://www.alexthompson.com",
                                            Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "1904 Niagara Stone Rd",
                                                AddressLine2 = "Distillery Lane",
                                                City = "Niagara-on-the-Lake",
                                                StateProvince = "ON",
                                                PostalCode = "L0S 1J0",
                                                Country = "Canada"
                                            }
                                        }
                                        },
                                    new Member
                                    {
                                        ID = 62,
                                        MemberName = "The House of Jerky",
                                        MemberSize = 6,
                                        JoinDate = new DateTime(2021, 3, 5),
                                        WebsiteUrl = "https://www.susanwilliams.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "120 Main St E",
                                                AddressLine2 = "Unit 3",
                                                City = "Grimsby",
                                                StateProvince = "ON",
                                                PostalCode = "L3M 1P3",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 63,
                                        MemberName = "Port Niagara Supplies",
                                        MemberSize = 12,
                                        JoinDate = new DateTime(2020, 12, 8),
                                        WebsiteUrl = "https://www.nathanharris.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "500 Port Rd",
                                                AddressLine2 = "Warehouse 4",
                                                City = "Port Colborne",
                                                StateProvince = "ON",
                                                PostalCode = "L3K 3T2",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 64,
                                        MemberName = "Hamilton Fabricators",
                                        MemberSize = 18,
                                        JoinDate = new DateTime(2021, 7, 22),
                                        WebsiteUrl = "https://www.davidmiller.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "8200 Park Rd",
                                                AddressLine2 = "Steelworks Building",
                                                City = "Stoney Creek",
                                                StateProvince = "ON",
                                                PostalCode = "L8E 5R2",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 65,
                                        MemberName = "Niagara Freight Forwarders",
                                        MemberSize = 16,
                                        JoinDate = new DateTime(2021, 9, 18),
                                        WebsiteUrl = "https://www.kendalljohnson.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "2173 Merrittville Hwy",
                                                AddressLine2 = "Freight Office",
                                                City = "Thorold",
                                                StateProvince = "ON",
                                                PostalCode = "L2V 1A1",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 66,
                                        MemberName = "Fort Erie Construction Ltd.",
                                        MemberSize = 14,
                                        JoinDate = new DateTime(2022, 4, 5),
                                        WebsiteUrl = "https://www.michaelscott.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "2567 Dominion Rd",
                                                AddressLine2 = "Building A",
                                                City = "Fort Erie",
                                                StateProvince = "ON",
                                                PostalCode = "L2A 1E5",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 67,
                                        MemberName = "Niagara Water Services",
                                        MemberSize = 9,
                                        JoinDate = new DateTime(2020, 11, 12),
                                        WebsiteUrl = "https://www.lucasbrown.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "7600 South Service Rd",
                                                AddressLine2 = "Water Distribution Centre",
                                                City = "Grimsby",
                                                StateProvince = "ON",
                                                PostalCode = "L3M 2Z1",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 68,
                                        MemberName = "Summit Sports Equipment",
                                        MemberSize = 13,
                                        JoinDate = new DateTime(2022, 2, 2),
                                        WebsiteUrl = "https://www.sophiareid.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "1450 Mountain Rd",
                                                AddressLine2 = "Sporting Goods Store",
                                                City = "Niagara Falls",
                                                StateProvince = "ON",
                                                PostalCode = "L2G 1X9",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 69,
                                        MemberName = "Niagara Waterpark Group",
                                        MemberSize = 24,
                                        JoinDate = new DateTime(2021, 6, 15),
                                        WebsiteUrl = "https://www.annaevans.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "2001 Park Rd",
                                                AddressLine2 = "Waterpark Entrance",
                                                City = "Niagara Falls",
                                                StateProvince = "ON",
                                                PostalCode = "L2E 6T1",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 70,
                                        MemberName = "Niagara Adventure Tours",
                                        MemberSize = 8,
                                        JoinDate = new DateTime(2022, 7, 25),
                                        WebsiteUrl = "https://www.williamroberts.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "1786 Falls Ave",
                                                AddressLine2 = "Tour Operator HQ",
                                                City = "Niagara Falls",
                                                StateProvince = "ON",
                                                PostalCode = "L2E 6V9",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 71,
                                        MemberName = "Kettle Creek Logistics",
                                        MemberSize = 20,
                                        JoinDate = new DateTime(2022, 3, 18),
                                        WebsiteUrl = "https://www.ryanjames.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "8459 Creek Rd",
                                                AddressLine2 = "Logistics Centre",
                                                City = "Niagara-on-the-Lake",
                                                StateProvince = "ON",
                                                PostalCode = "L0S 1J0",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 72,
                                        MemberName = "Niagara Custom Carpentry",
                                        MemberSize = 12,
                                        JoinDate = new DateTime(2021, 1, 28),
                                        WebsiteUrl = "https://www.emilydavis.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "3125 Highway 20",
                                                AddressLine2 = "Woodworking Shop",
                                                City = "Thorold",
                                                StateProvince = "ON",
                                                PostalCode = "L2V 3M4",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 73,
                                        MemberName = "Greenstone Landscaping",
                                        MemberSize = 7,
                                        JoinDate = new DateTime(2022, 5, 8),
                                        WebsiteUrl = "https://www.sophiebaker.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "1349 Greenstone Rd",
                                                AddressLine2 = "Landscaping Services",
                                                City = "St. Catharines",
                                                StateProvince = "ON",
                                                PostalCode = "L2M 3W3",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 74,
                                        MemberName = "Niagara Marketing Group",
                                        MemberSize = 10,
                                        JoinDate = new DateTime(2021, 4, 1),
                                        WebsiteUrl = "https://www.kennethgonzalez.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "1550 King St",
                                                AddressLine2 = "Marketing Agency",
                                                City = "Niagara Falls",
                                                StateProvince = "ON",
                                                PostalCode = "L2G 1J7",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 75,
                                        MemberName = "Elmwood Construction",
                                        MemberSize = 8,
                                        JoinDate = new DateTime(2022, 5, 15),
                                        WebsiteUrl = "https://www.alexanderjohnson.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "500 Elmwood Ave",
                                                AddressLine2 = "Construction Office",
                                                City = "Welland",
                                                StateProvince = "ON",
                                                PostalCode = "L3C 1W1",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 76,
                                        MemberName = "Harbourview Cafe",
                                        MemberSize = 4,
                                        JoinDate = new DateTime(2021, 11, 2),
                                        WebsiteUrl = "https://www.jonathandoe.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "5100 Lakeshore Rd",
                                                AddressLine2 = "Cafe Front",
                                                City = "Port Colborne",
                                                StateProvince = "ON",
                                                PostalCode = "L3K 5V3",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 77,
                                        MemberName = "Niagara River Cruises",
                                        MemberSize = 15,
                                        JoinDate = new DateTime(2021, 10, 11),
                                        WebsiteUrl = "https://www.isaacmoore.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "123 Niagara Pkwy",
                                                AddressLine2 = "Cruise Dock",
                                                City = "Niagara-on-the-Lake",
                                                StateProvince = "ON",
                                                PostalCode = "L0S 1J0",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 78,
                                        MemberName = "Main Street Bistro",
                                        MemberSize = 6,
                                        JoinDate = new DateTime(2021, 9, 5),
                                        WebsiteUrl = "https://www.maryjackson.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "435 Main St",
                                                AddressLine2 = "Bistro Shop",
                                                City = "Grimsby",
                                                StateProvince = "ON",
                                                PostalCode = "L3M 1P1",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 79,
                                        MemberName = "Clearwater Springs Lodge",
                                        MemberSize = 28,
                                        JoinDate = new DateTime(2021, 12, 15),
                                        WebsiteUrl = "https://www.olivermiller.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "2252 Lakeshore Rd",
                                                AddressLine2 = "Lodge Entrance",
                                                City = "Fort Erie",
                                                StateProvince = "ON",
                                                PostalCode = "L2A 1G2",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 80,
                                        MemberName = "The Stone Oven Bakery",
                                        MemberSize = 5,
                                        JoinDate = new DateTime(2022, 1, 17),
                                        WebsiteUrl = "https://www.jamesroberts.com",
                                        Addresses = new List<Address>
                                        {
                                            new Address
                                            {
                                                AddressLine1 = "100 Main St",
                                                AddressLine2 = "Bakery Front",
                                                City = "Niagara-on-the-Lake",
                                                StateProvince = "ON",
                                                PostalCode = "L0S 1J0",
                                                Country = "Canada"
                                            }
                                        }
                                    },
                                    new Member
                                    {
                                        ID = 81,
                                        MemberName = "Golden Oaks Winery",
                                        MemberSize = 11,
                                        JoinDate = new DateTime(2022, 2, 10),
                                        WebsiteUrl = "https://www.jenniferhill.com",
                                        Addresses = new List<Address>
                                    {
                                        new Address
                                        {
                                            AddressLine1 = "1234 Golden Rd",
                                            AddressLine2 = "Tasting Room",
                                            City = "Niagara-on-the-Lake",
                                            StateProvince = "ON",
                                            PostalCode = "L0S 1J0",
                                            Country = "Canada"
                                        }
                                    }
                                    },
                                new Member
                                {
                                    ID = 82,
                                    MemberName = "Cedar Ridge Rentals",
                                    MemberSize = 10,
                                    JoinDate = new DateTime(2021, 8, 19),
                                    WebsiteUrl = "https://www.joemartinez.com",
                                    Addresses = new List<Address>
                                    {
                                        new Address
                                        {
                                            AddressLine1 = "1000 Ridge Rd",
                                            AddressLine2 = "Rental Office",
                                            City = "St. Catharines",
                                            StateProvince = "ON",
                                            PostalCode = "L2P 3R3",
                                            Country = "Canada"
                                        }
                                    }
                                },
                                new Member
                                {
                                    ID = 83,
                                    MemberName = "Silverstone Golf Club",
                                    MemberSize = 35,
                                    JoinDate = new DateTime(2021, 5, 14),
                                    WebsiteUrl = "https://www.christopheranderson.com",
                                    Addresses = new List<Address>
                                    {
                                        new Address
                                        {
                                            AddressLine1 = "500 Silverstone Dr",
                                            AddressLine2 = "Clubhouse",
                                            City = "Niagara Falls",
                                            StateProvince = "ON",
                                            PostalCode = "L2E 6V1",
                                            Country = "Canada"
                                        }
                                    }
                                },
                                new Member
                                {
                                    ID = 84,
                                    MemberName = "Napa Valley Art Gallery",
                                    MemberSize = 6,
                                    JoinDate = new DateTime(2022, 1, 23),
                                    WebsiteUrl = "https://www.briannawilliams.com",
                                    Addresses = new List<Address>
                                    {
                                        new Address
                                        {
                                            AddressLine1 = "222 Art St",
                                            AddressLine2 = "Gallery Showroom",
                                            City = "Niagara-on-the-Lake",
                                            StateProvince = "ON",
                                            PostalCode = "L0S 1J0",
                                            Country = "Canada"
                                        }
                                    }
                                },
                                new Member
                                {
                                    ID = 85,
                                    MemberName = "Waterfront Bites Restaurant",
                                    MemberSize = 15,
                                    JoinDate = new DateTime(2021, 10, 2),
                                    WebsiteUrl = "https://www.daniellawson.com",
                                    Addresses = new List<Address>
                                    {
                                        new Address
                                        {
                                            AddressLine1 = "345 Lake Rd",
                                            AddressLine2 = "Restaurant Dining",
                                            City = "Port Colborne",
                                            StateProvince = "ON",
                                            PostalCode = "L3K 3Y6",
                                            Country = "Canada"
                                        }
                                    }
                                },
                                new Member
                                {
                                    ID = 86,
                                    MemberName = "Starlight Cinemas",
                                    MemberSize = 50,
                                    JoinDate = new DateTime(2020, 11, 28),
                                    WebsiteUrl = "https://www.ashleymorris.com",
                                    Addresses = new List<Address>
                                    {
                                        new Address
                                        {
                                            AddressLine1 = "2200 Main St",
                                            AddressLine2 = "Cinema Entrance",
                                            City = "Niagara Falls",
                                            StateProvince = "ON",
                                            PostalCode = "L2G 1J4",
                                            Country = "Canada"
                                        }
                                    }
                                },
                                new Member
                                {
                                    ID = 87,
                                    MemberName = "Niagara Fitness Club",
                                    MemberSize = 30,
                                    JoinDate = new DateTime(2021, 6, 30),
                                    WebsiteUrl = "https://www.jessicaperez.com",
                                    Addresses = new List<Address>
                                    {
                                        new Address
                                        {
                                            AddressLine1 = "1585 Fitness Rd",
                                            AddressLine2 = "Gym Entrance",
                                            City = "St. Catharines",
                                            StateProvince = "ON",
                                            PostalCode = "L2R 1C9",
                                            Country = "Canada"
                                        }
                                    }
                                },
                                new Member
                                {
                                    ID = 88,
                                    MemberName = "Rolling Hills Construction",
                                    MemberSize = 20,
                                    JoinDate = new DateTime(2022, 3, 10),
                                    WebsiteUrl = "https://www.jordanpeterson.com",
                                    Addresses = new List<Address>
                                    {
                                        new Address
                                        {
                                            AddressLine1 = "1196 Rolling Hills Rd",
                                            AddressLine2 = "Construction Site",
                                            City = "Welland",
                                            StateProvince = "ON",
                                            PostalCode = "L3B 4K9",
                                            Country = "Canada"
                                        }
                                    }
                                },
                                new Member
                                {
                                    ID = 89,
                                    MemberName = "Firefly Electronics",
                                    MemberSize = 10,
                                    JoinDate = new DateTime(2021, 4, 25),
                                    WebsiteUrl = "https://www.marykline.com",
                                    Addresses = new List<Address>
                                    {
                                        new Address
                                        {
                                            AddressLine1 = "789 Tech Rd",
                                            AddressLine2 = "Electronics HQ",
                                            City = "Grimsby",
                                            StateProvince = "ON",
                                            PostalCode = "L3M 4R2",
                                            Country = "Canada"
                                        }
                                    }
                                },
                                new Member
                                {
                                    ID = 90,
                                    MemberName = "Crystal Clear Pools",
                                    MemberSize = 18,
                                    JoinDate = new DateTime(2022, 6, 15),
                                    WebsiteUrl = "https://www.nicholasjohnson.com",
                                    Addresses = new List<Address>
                                    {
                                        new Address
                                        {
                                            AddressLine1 = "1200 Crystal Blvd",
                                            AddressLine2 = "Pool Services",
                                            City = "Niagara Falls",
                                            StateProvince = "ON",
                                            PostalCode = "L2E 1P8",
                                            Country = "Canada"
                                        }
                                    }
                                },
                                new Member
                                {
                                    ID = 91,
                                    MemberName = "Niagara Soapworks",
                                    MemberSize = 8,
                                    JoinDate = new DateTime(2022, 4, 18),
                                    WebsiteUrl = "https://www.lauranorris.com",
                                    Addresses = new List<Address>
                                    {
                                        new Address
                                        {
                                            AddressLine1 = "234 Soap Rd",
                                            AddressLine2 = "Soap Factory",
                                            City = "Niagara-on-the-Lake",
                                            StateProvince = "ON",
                                            PostalCode = "L0S 1J0",
                                            Country = "Canada"
                                        }
                                    }
                                },
                                new Member
                                {
                                    ID = 92,
                                    MemberName = "Pinehurst Brewing Company",
                                    MemberSize = 20,
                                    JoinDate = new DateTime(2021, 9, 23),
                                    WebsiteUrl = "https://www.hannahbrooks.com",
                                    Addresses = new List<Address>
                                    {
                                        new Address
                                        {
                                            AddressLine1 = "567 Pinehurst Rd",
                                            AddressLine2 = "Brewery Entrance",
                                            City = "Thorold",
                                            StateProvince = "ON",
                                            PostalCode = "L2V 1A9",
                                            Country = "Canada"
                                        }
                                    }
                                },
                                new Member
                                {
                                    ID = 93,
                                    MemberName = "Niagara Ice Creamery",
                                    MemberSize = 5,
                                    JoinDate = new DateTime(2021, 12, 20),
                                    WebsiteUrl = "https://www.danielpatel.com",
                                    Addresses = new List<Address>
                                    {
                                        new Address
                                        {
                                            AddressLine1 = "1346 Creamery Rd",
                                            AddressLine2 = "Ice Cream Shop",
                                            City = "Niagara Falls",
                                            StateProvince = "ON",
                                            PostalCode = "L2E 6T3",
                                            Country = "Canada"
                                        }
                                    }
                                },
                                new Member
                                {
                                    ID = 94,
                                    MemberName = "Green Valley Farms",
                                    MemberSize = 14,
                                    JoinDate = new DateTime(2022, 2, 28),
                                    WebsiteUrl = "https://www.amandaevans.com",
                                    Addresses = new List<Address>
                                    {
                                        new Address
                                        {
                                            AddressLine1 = "2345 Green Valley Rd",
                                            AddressLine2 = "Farm Shop",
                                            City = "St. Catharines",
                                            StateProvince = "ON",
                                            PostalCode = "L2P 3J5",
                                            Country = "Canada"
                                        }
                                    }
                                },
                                new Member
                                {
                                    ID = 95,
                                    MemberName = "Lakefront Wine Cellars",
                                    MemberSize = 10,
                                    JoinDate = new DateTime(2021, 8, 12),
                                    WebsiteUrl = "https://www.rachelwhitman.com",
                                    Addresses = new List<Address>
                                    {
                                        new Address
                                        {
                                            AddressLine1 = "665 Lakeview Rd",
                                            AddressLine2 = "Tasting Room",
                                            City = "Niagara-on-the-Lake",
                                            StateProvince = "ON",
                                            PostalCode = "L0S 1J0",
                                            Country = "Canada"
                                        }
                                    }
                                },
                                new Member
                                {
                                    ID = 96,
                                    MemberName = "Vineyard View Estates",
                                    MemberSize = 25,
                                    JoinDate = new DateTime(2022, 5, 7),
                                    WebsiteUrl = "https://www.oliviagray.com",
                                    Addresses = new List<Address>
                                {
                                    new Address
                                    {
                                        AddressLine1 = "888 Vineyard Dr",
                                        AddressLine2 = "Winery Entrance",
                                        City = "Niagara-on-the-Lake",
                                        StateProvince = "ON",
                                        PostalCode = "L0S 1J0",
                                        Country = "Canada"
                                    }
                                }
                                },
                            new Member
                            {
                                ID = 97,
                                MemberName = "Pinewood Resorts",
                                MemberSize = 40,
                                JoinDate = new DateTime(2021, 11, 15),
                                WebsiteUrl = "https://www.juliajones.com",
                                Addresses = new List<Address>
                                {
                                    new Address
                                    {
                                        AddressLine1 = "1420 Pinewood Ln",
                                        AddressLine2 = "Resort Main Office",
                                        City = "Welland",
                                        StateProvince = "ON",
                                        PostalCode = "L3B 2H6",
                                        Country = "Canada"
                                    }
                                }
                            },
                            new Member
                            {
                                ID = 98,
                                MemberName = "Riverside Marinas",
                                MemberSize = 12,
                                JoinDate = new DateTime(2022, 8, 1),
                                WebsiteUrl = "https://www.matthewharris.com",
                                Addresses = new List<Address>
                                {
                                    new Address
                                    {
                                        AddressLine1 = "750 Riverside Dr",
                                        AddressLine2 = "Marina Office",
                                        City = "Port Colborne",
                                        StateProvince = "ON",
                                        PostalCode = "L3K 5C3",
                                        Country = "Canada"
                                    }
                                }
                            },
                            new Member
                            {
                                ID = 99,
                                MemberName = "Niagara Craft Distillery",
                                MemberSize = 8,
                                JoinDate = new DateTime(2021, 7, 18),
                                WebsiteUrl = "https://www.kaylathompson.com",
                                Addresses = new List<Address>
                                {
                                    new Address
                                    {
                                        AddressLine1 = "346 Distillery Rd",
                                        AddressLine2 = "Distillery Shop",
                                        City = "Niagara-on-the-Lake",
                                        StateProvince = "ON",
                                        PostalCode = "L0S 1J0",
                                        Country = "Canada"
                                    }
                                }
                            },
                            new Member
                            {
                                ID = 100,
                                MemberName = "Heritage Hotels & Resorts",
                                MemberSize = 65,
                                JoinDate = new DateTime(2022, 3, 5),
                                WebsiteUrl = "https://www.zoemorris.com",
                                Addresses = new List<Address>
                                {
                                    new Address
                                    {
                                        AddressLine1 = "200 Heritage Ln",
                                        AddressLine2 = "Hotel Main Entrance",
                                        City = "Niagara Falls",
                                        StateProvince = "ON",
                                        PostalCode = "L2G 1P8",
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
                        // MembershipTypeIds (from 1 to 5, corresponding to different types)
                        int[] membershipTypeIds = new int[] { 1, 2, 3, 4, 5 }; // Basic, Premium, Family, Student, Corporate

                        // Create a random generator
                        Random random = new Random();

                        // Initialize a list to hold the MemberMembershipType entries
                        List<MemberMembershipType> memberMembershipTypes = new List<MemberMembershipType>();

                        // Loop over the member IDs (from 1 to 100)
                        for (int memberId = 1; memberId <= 100; memberId++)
                        {
                            // Randomly pick a MembershipTypeId for each member
                            int randomMembershipTypeId = membershipTypeIds[random.Next(membershipTypeIds.Length)];

                            // Add the random membership type to the list
                            memberMembershipTypes.Add(new MemberMembershipType
                            {
                                MemberId = memberId,
                                MembershipTypeId = randomMembershipTypeId
                            });
                        }

                        // Save the data to the context
                        context.MemberMembershipTypes.AddRange(memberMembershipTypes);
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
                        },
                        new Contact
                        {
                            Id = 41,
                            FirstName = "Astarion",
                            LastName = "Ancunin",
                            Title = "Manager",
                            Department = "Sales",
                            Email = "iplayedbg3toomuch@example.com",
                            Phone = "7077077777",
                            LinkedInUrl = "https://www.linkedin.com/in/johndoe",
                            IsVip = false,
                            MemberId = 41
                        },
                        new Contact
                        {
                            Id = 42,
                            FirstName = "John",
                            LastName = "Thomas",
                            Title = "Manager",
                            Department = "Sales",
                            Email = "john.doe@example.com",
                            Phone = "1234567890",
                            LinkedInUrl = "https://www.linkedin.com/in/johndoe",
                            IsVip = true,
                            MemberId = 42
                        },

                            new Contact
                            {
                                Id = 43,
                                FirstName = "Jane",
                                LastName = "Smith",
                                Title = "Director",
                                Department = "Marketing",
                                Email = "jane.smith@example.com",
                                Phone = "9876543210",
                                LinkedInUrl = "https://www.linkedin.com/in/janesmith",
                                IsVip = false,
                                MemberId = 43
                            },
                            new Contact
                            {
                                Id = 44,
                                FirstName = "Alice",
                                LastName = "Johnson",
                                Title = "VP",
                                Department = "Human Resources",
                                Email = "alice.johnson@example.com",
                                Phone = "5551234567",
                                LinkedInUrl = "https://www.linkedin.com/in/alicejohnson",
                                IsVip = true,
                                MemberId = 44
                            },
                            new Contact
                            {
                                Id = 45,
                                FirstName = "Bob",
                                LastName = "Joe",
                                Title = "Chief Financial Officer",
                                Department = "Finance",
                                Email = "bob.brown@example.com",
                                Phone = "5557654321",
                                LinkedInUrl = "https://www.linkedin.com/in/bobbrown",
                                IsVip = true,
                                MemberId = 45
                            },
                            new Contact
                            {
                                Id = 46,
                                FirstName = "Charlie",
                                LastName = "Davis",
                                Title = "Chief Operating Officer",
                                Department = "Operations",
                                Email = "charlie.davis@example.com",
                                Phone = "5557890123",
                                LinkedInUrl = "https://www.linkedin.com/in/charliedavis",
                                IsVip = false,
                                MemberId = 46
                            },
                            new Contact
                            {
                                Id = 47,
                                FirstName = "Deborah",
                                LastName = "Williams",
                                Title = "Director of Technology",
                                Department = "Technology",
                                Email = "deborah.williams@example.com",
                                Phone = "5552345678",
                                LinkedInUrl = "https://www.linkedin.com/in/deborahwilliams",
                                IsVip = true,
                                MemberId = 47
                            },
                            new Contact
                            {
                                Id = 48,
                                FirstName = "Eve",
                                LastName = "Marie",
                                Title = "Marketing Specialist",
                                Department = "Marketing",
                                Email = "eve.taylor@example.com",
                                Phone = "5553456789",
                                LinkedInUrl = "https://www.linkedin.com/in/evetaylor",
                                IsVip = false,
                                MemberId = 48
                            },
                            new Contact
                            {
                                Id = 49,
                                FirstName = "Frank",
                                LastName = "Harris",
                                Title = "Senior Engineer",
                                Department = "Engineering",
                                Email = "frank.harris@example.com",
                                Phone = "5554567890",
                                LinkedInUrl = "https://www.linkedin.com/in/frankharris",
                                IsVip = true,
                                MemberId = 49
                            },
                            new Contact
                            {
                                Id = 50,
                                FirstName = "Grace",
                                LastName = "King",
                                Title = "Business Development Manager",
                                Department = "Sales",
                                Email = "grace.king@example.com",
                                Phone = "5555678901",
                                LinkedInUrl = "https://www.linkedin.com/in/graceking",
                                IsVip = false,
                                MemberId = 50
                            },
                            new Contact
                            {
                                Id = 51,
                                FirstName = "Hank",
                                LastName = "Lee",
                                Title = "Head of Research",
                                Department = "Research and Development",
                                Email = "hank.lee@example.com",
                                Phone = "5556789012",
                                LinkedInUrl = "https://www.linkedin.com/in/hanklee",
                                IsVip = true,
                                MemberId = 51
                            },
                            new Contact
                            {
                                Id = 52,
                                FirstName = "Ivy",
                                LastName = "Adams",
                                Title = "Project Manager",
                                Department = "Operations",
                                Email = "ivy.adams@example.com",
                                Phone = "5557890123",
                                LinkedInUrl = "https://www.linkedin.com/in/ivyadams",
                                IsVip = false,
                                MemberId = 52
                            },
                            new Contact
                            {
                                Id = 53,
                                FirstName = "Jack",
                                LastName = "Scott",
                                Title = "CEO",
                                Department = "Executive",
                                Email = "jack.scott@example.com",
                                Phone = "5558901234",
                                LinkedInUrl = "https://www.linkedin.com/in/jackscott",
                                IsVip = true,
                                MemberId = 53
                            },
                            new Contact
                            {
                                Id = 54,
                                FirstName = "Kathy",
                                LastName = "Elizabeth",
                                Title = "HR Specialist",
                                Department = "Human Resources",
                                Email = "kathy.morris@example.com",
                                Phone = "5559012345",
                                LinkedInUrl = "https://www.linkedin.com/in/kathymorris",
                                IsVip = false,
                                MemberId = 54
                            },
                            new Contact
                            {
                                Id = 55,
                                FirstName = "Louis",
                                LastName = "Alexandr",
                                Title = "Customer Service Lead",
                                Department = "Customer Service",
                                Email = "louis.walker@example.com",
                                Phone = "5550123456",
                                LinkedInUrl = "https://www.linkedin.com/in/louiswalker",
                                IsVip = true,
                                MemberId = 55
                            },
                            new Contact
                            {
                                Id = 56,
                                FirstName = "Mona",
                                LastName = "Grace",
                                Title = "Legal Advisor",
                                Department = "Legal",
                                Email = "mona.white@example.com",
                                Phone = "5551234567",
                                LinkedInUrl = "https://www.linkedin.com/in/monawhite",
                                IsVip = false,
                                MemberId = 56
                            },
                            new Contact
                            {
                                Id = 57,
                                FirstName = "James",
                                LastName = "T.",
                                Title = "Marketing Manager",
                                Department = "Marketing",
                                Email = "james.smith@example.com",
                                Phone = "5559876543",
                                LinkedInUrl = "https://www.linkedin.com/in/jamessmith",
                                IsVip = false,
                                MemberId = 57
                            },
                            new Contact
                            {
                                Id = 58,
                                FirstName = "Sarah",
                                LastName = "A.",
                                Title = "HR Specialist",
                                Department = "Human Resources",
                                Email = "sarah.johnson@example.com",
                                Phone = "5551122334",
                                LinkedInUrl = "https://www.linkedin.com/in/sarahjohnson",
                                IsVip = false,
                                MemberId = 58
                            },
                            new Contact
                            {
                                Id = 59,
                                FirstName = "DavId",
                                LastName = "L.",
                                Title = "Chief Executive Officer",
                                Department = "Executive",
                                Email = "davId.brown@example.com",
                                Phone = "5552233445",
                                LinkedInUrl = "https://www.linkedin.com/in/davIdbrown",
                                IsVip = true,
                                MemberId = 59
                            },
                            new Contact
                            {
                                Id = 60,
                                FirstName = "Emily",
                                LastName = "M.",
                                Title = "Product Designer",
                                Department = "Design",
                                Email = "emily.williams@example.com",
                                Phone = "5556677889",
                                LinkedInUrl = "https://www.linkedin.com/in/emilywilliams",
                                IsVip = false,
                                MemberId = 60
                            },
                            new Contact
                            {
                                Id = 61,
                                FirstName = "Michael",
                                LastName = "J.",
                                Title = "Sales Director",
                                Department = "Sales",
                                Email = "michael.davis@example.com",
                                Phone = "5558899001",
                                LinkedInUrl = "https://www.linkedin.com/in/michaeldavis",
                                IsVip = false,
                                MemberId = 61
                            },
                            new Contact
                            {
                                Id = 62,
                                FirstName = "Olivia",
                                LastName = "K.",
                                Title = "Chief Financial Officer",
                                Department = "Finance",
                                Email = "olivia.martinez@example.com",
                                Phone = "5553456789",
                                LinkedInUrl = "https://www.linkedin.com/in/oliviamartinez",
                                IsVip = true,
                                MemberId = 62
                            },
                            new Contact
                            {
                                Id = 63,
                                FirstName = "Ethan",
                                LastName = "B.",
                                Title = "IT Manager",
                                Department = "IT",
                                Email = "ethan.taylor@example.com",
                                Phone = "5552345678",
                                LinkedInUrl = "https://www.linkedin.com/in/ethantaylor",
                                IsVip = false,
                                MemberId = 63
                            },
                            new Contact
                            {
                                Id = 64,
                                FirstName = "Sophia",
                                LastName = "J.",
                                Title = "Operations Coordinator",
                                Department = "Operations",
                                Email = "sophia.wilson@example.com",
                                Phone = "5556781234",
                                LinkedInUrl = "https://www.linkedin.com/in/sophiawilson",
                                IsVip = false,
                                MemberId = 64
                            },
                            new Contact
                            {
                                Id = 65,
                                FirstName = "Daniel",
                                LastName = "P.",
                                Title = "Customer Success Manager",
                                Department = "Customer Support",
                                Email = "daniel.moore@example.com",
                                Phone = "5559988776",
                                LinkedInUrl = "https://www.linkedin.com/in/danielmoore",
                                IsVip = false,
                                MemberId = 65
                            },
                        new Contact
                        {
                            Id = 66,
                            FirstName = "Chloe",
                            LastName = "S.",
                            Title = "Senior Analyst",
                            Department = "Finance",
                            Email = "chloe.martin@example.com",
                            Phone = "5557766554",
                            LinkedInUrl = "https://www.linkedin.com/in/chloemartin",
                            IsVip = true,
                            MemberId = 66
                        },
                        new Contact
                        {
                            Id = 67,
                            FirstName = "Liam",
                            LastName = "G.",
                            Title = "Project Manager",
                            Department = "Operations",
                            Email = "liam.green@example.com",
                            Phone = "5554433221",
                            LinkedInUrl = "https://www.linkedin.com/in/liamgreen",
                            IsVip = false,
                            MemberId = 67
                        },
                        new Contact
                        {
                            Id = 68,
                            FirstName = "Isabella",
                            LastName = "H.",
                            Title = "Marketing Director",
                            Department = "Marketing",
                            Email = "isabella.hudson@example.com",
                            Phone = "5559988776",
                            LinkedInUrl = "https://www.linkedin.com/in/isabellahudson",
                            IsVip = true,
                            MemberId = 68
                        },
                        new Contact
                        {
                            Id = 69,
                            FirstName = "Ethan",
                            LastName = "P.",
                            Title = "Sales Manager",
                            Department = "Sales",
                            Email = "ethan.peters@example.com",
                            Phone = "5551122334",
                            LinkedInUrl = "https://www.linkedin.com/in/ethanpeters",
                            IsVip = false,
                            MemberId = 69
                        },
                        new Contact
                        {
                            Id = 70,
                            FirstName = "Ava",
                            LastName = "W.",
                            Title = "Human Resources Specialist",
                            Department = "HR",
                            Email = "ava.williams@example.com",
                            Phone = "5556677889",
                            LinkedInUrl = "https://www.linkedin.com/in/avawilliams",
                            IsVip = true,
                            MemberId = 70
                        },
                        new Contact
                        {
                            Id = 71,
                            FirstName = "Mason",
                            LastName = "J.",
                            Title = "Data Scientist",
                            Department = "IT",
                            Email = "mason.james@example.com",
                            Phone = "5554455667",
                            LinkedInUrl = "https://www.linkedin.com/in/masonjames",
                            IsVip = false,
                            MemberId = 71
                        },
                        new Contact
                        {
                            Id = 72,
                            FirstName = "Sophia",
                            LastName = "K.",
                            Title = "Customer Support Lead",
                            Department = "Customer Service",
                            Email = "sophia.king@example.com",
                            Phone = "5552334455",
                            LinkedInUrl = "https://www.linkedin.com/in/sophiaking",
                            IsVip = true,
                            MemberId = 72
                        },
                        new Contact
                        {
                            Id = 73,
                            FirstName = "Jackson",
                            LastName = "T.",
                            Title = "Chief Technology Officer",
                            Department = "Technology",
                            Email = "jackson.taylor@example.com",
                            Phone = "5555555555",
                            LinkedInUrl = "https://www.linkedin.com/in/jacksontaylor",
                            IsVip = true,
                            MemberId = 73
                        },
                        new Contact
                        {
                            Id = 74,
                            FirstName = "Charlotte",
                            LastName = "L.",
                            Title = "Finance Manager",
                            Department = "Finance",
                            Email = "charlotte.larson@example.com",
                            Phone = "5552233446",
                            LinkedInUrl = "https://www.linkedin.com/in/charlottelarson",
                            IsVip = false,
                            MemberId = 74
                        },
                        new Contact
                        {
                            Id = 75,
                            FirstName = "Lucas",
                            LastName = "B.",
                            Title = "IT Specialist",
                            Department = "IT",
                            Email = "lucas.brown@example.com",
                            Phone = "5556677880",
                            LinkedInUrl = "https://www.linkedin.com/in/lucasbrown",
                            IsVip = true,
                            MemberId = 75
                        },
                        new Contact
                        {
                            Id = 76,
                            FirstName = "Mia",
                            LastName = "C.",
                            Title = "Legal Advisor",
                            Department = "Legal",
                            Email = "mia.carter@example.com",
                            Phone = "5553334446",
                            LinkedInUrl = "https://www.linkedin.com/in/miacarter",
                            IsVip = true,
                            MemberId = 76
                        },
                        new Contact
                        {
                            Id = 77,
                            FirstName = "Logan",
                            LastName = "D.",
                            Title = "Operations Manager",
                            Department = "Operations",
                            Email = "logan.davis@example.com",
                            Phone = "5553344556",
                            LinkedInUrl = "https://www.linkedin.com/in/logandavis",
                            IsVip = false,
                            MemberId = 77
                        },
                        new Contact
                        {
                            Id = 78,
                            FirstName = "Harper",
                            LastName = "M.",
                            Title = "Product Manager",
                            Department = "Product",
                            Email = "harper.morris@example.com",
                            Phone = "5555566778",
                            LinkedInUrl = "https://www.linkedin.com/in/harpermorris",
                            IsVip = true,
                            MemberId = 78
                        },
                        new Contact
                        {
                            Id = 79,
                            FirstName = "Benjamin",
                            LastName = "N.",
                            Title = "Chief Executive Officer",
                            Department = "Executive",
                            Email = "benjamin.nelson@example.com",
                            Phone = "5551223344",
                            LinkedInUrl = "https://www.linkedin.com/in/benjaminnelson",
                            IsVip = true,
                            MemberId = 79
                        },
                        new Contact
                        {
                            Id = 80,
                            FirstName = "Ella",
                            LastName = "O.",
                            Title = "Public Relations Manager",
                            Department = "PR",
                            Email = "ella.olson@example.com",
                            Phone = "5554455668",
                            LinkedInUrl = "https://www.linkedin.com/in/ellaolson",
                            IsVip = false,
                            MemberId = 80
                        },
                        new Contact
                        {
                            Id = 81,
                            FirstName = "James",
                            LastName = "P.",
                            Title = "Chief Marketing Officer",
                            Department = "Marketing",
                            Email = "james.phillips@example.com",
                            Phone = "5557768999",
                            LinkedInUrl = "https://www.linkedin.com/in/jamesphillips",
                            IsVip = true,
                            MemberId = 81
                        },
                        new Contact
                        {
                            Id = 82,
                            FirstName = "John",
                            LastName = "Thomas",
                            Title = "Manager",
                            Department = "Sales",
                            Email = "john.doe@example.com",
                            Phone = "1234567890",
                            LinkedInUrl = "https://www.linkedin.com/in/johndoe",
                            IsVip = true,
                            MemberId = 82
                        },

                            new Contact
                            {
                                Id = 83,
                                FirstName = "Jane",
                                LastName = "Smith",
                                Title = "Director",
                                Department = "Marketing",
                                Email = "jane.smith@example.com",
                                Phone = "9876543210",
                                LinkedInUrl = "https://www.linkedin.com/in/janesmith",
                                IsVip = false,
                                MemberId = 83
                            },
                            new Contact
                            {
                                Id = 84,
                                FirstName = "Alice",
                                LastName = "Johnson",
                                Title = "VP",
                                Department = "Human Resources",
                                Email = "alice.johnson@example.com",
                                Phone = "5551234567",
                                LinkedInUrl = "https://www.linkedin.com/in/alicejohnson",
                                IsVip = true,
                                MemberId = 84
                            },
                            new Contact
                            {
                                Id = 85,
                                FirstName = "Bob",
                                LastName = "Joe",
                                Title = "Chief Financial Officer",
                                Department = "Finance",
                                Email = "bob.brown@example.com",
                                Phone = "5557654321",
                                LinkedInUrl = "https://www.linkedin.com/in/bobbrown",
                                IsVip = true,
                                MemberId = 85
                            },
                            new Contact
                            {
                                Id = 86,
                                FirstName = "Charlie",
                                LastName = "Davis",
                                Title = "Chief Operating Officer",
                                Department = "Operations",
                                Email = "charlie.davis@example.com",
                                Phone = "5557890123",
                                LinkedInUrl = "https://www.linkedin.com/in/charliedavis",
                                IsVip = false,
                                MemberId = 86
                            },
                            new Contact
                            {
                                Id = 87,
                                FirstName = "Deborah",
                                LastName = "Williams",
                                Title = "Director of Technology",
                                Department = "Technology",
                                Email = "deborah.williams@example.com",
                                Phone = "5552345678",
                                LinkedInUrl = "https://www.linkedin.com/in/deborahwilliams",
                                IsVip = true,
                                MemberId = 87
                            },
                            new Contact
                            {
                                Id = 88,
                                FirstName = "Eve",
                                LastName = "Marie",
                                Title = "Marketing Specialist",
                                Department = "Marketing",
                                Email = "eve.taylor@example.com",
                                Phone = "5553456789",
                                LinkedInUrl = "https://www.linkedin.com/in/evetaylor",
                                IsVip = false,
                                MemberId = 88
                            },
                            new Contact
                            {
                                Id = 89,
                                FirstName = "Frank",
                                LastName = "Harris",
                                Title = "Senior Engineer",
                                Department = "Engineering",
                                Email = "frank.harris@example.com",
                                Phone = "5554567890",
                                LinkedInUrl = "https://www.linkedin.com/in/frankharris",
                                IsVip = true,
                                MemberId = 89
                            },
                            new Contact
                            {
                                Id = 90,
                                FirstName = "Grace",
                                LastName = "King",
                                Title = "Business Development Manager",
                                Department = "Sales",
                                Email = "grace.king@example.com",
                                Phone = "5555678901",
                                LinkedInUrl = "https://www.linkedin.com/in/graceking",
                                IsVip = false,
                                MemberId = 90
                            },
                            new Contact
                            {
                                Id = 91,
                                FirstName = "Hank",
                                LastName = "Lee",
                                Title = "Head of Research",
                                Department = "Research and Development",
                                Email = "hank.lee@example.com",
                                Phone = "5556789012",
                                LinkedInUrl = "https://www.linkedin.com/in/hanklee",
                                IsVip = true,
                                MemberId = 91
                            },
                            new Contact
                            {
                                Id = 92,
                                FirstName = "Ivy",
                                LastName = "Adams",
                                Title = "Project Manager",
                                Department = "Operations",
                                Email = "ivy.adams@example.com",
                                Phone = "5557890123",
                                LinkedInUrl = "https://www.linkedin.com/in/ivyadams",
                                IsVip = false,
                                MemberId = 92
                            },
                            new Contact
                            {
                                Id = 93,
                                FirstName = "Jack",
                                LastName = "Scott",
                                Title = "CEO",
                                Department = "Executive",
                                Email = "jack.scott@example.com",
                                Phone = "5558901234",
                                LinkedInUrl = "https://www.linkedin.com/in/jackscott",
                                IsVip = true,
                                MemberId = 93
                            },
                            new Contact
                            {
                                Id = 94,
                                FirstName = "Kathy",
                                LastName = "Elizabeth",
                                Title = "HR Specialist",
                                Department = "Human Resources",
                                Email = "kathy.morris@example.com",
                                Phone = "5559012345",
                                LinkedInUrl = "https://www.linkedin.com/in/kathymorris",
                                IsVip = false,
                                MemberId = 94
                            },
                            new Contact
                            {
                                Id = 95,
                                FirstName = "Louis",
                                LastName = "Alexandr",
                                Title = "Customer Service Lead",
                                Department = "Customer Service",
                                Email = "louis.walker@example.com",
                                Phone = "5550123456",
                                LinkedInUrl = "https://www.linkedin.com/in/louiswalker",
                                IsVip = true,
                                MemberId = 95
                            },
                            new Contact
                            {
                                Id = 96,
                                FirstName = "Mona",
                                LastName = "Grace",
                                Title = "Legal Advisor",
                                Department = "Legal",
                                Email = "mona.white@example.com",
                                Phone = "5551234567",
                                LinkedInUrl = "https://www.linkedin.com/in/monawhite",
                                IsVip = false,
                                MemberId = 96
                            },
                            new Contact
                            {
                                Id = 97,
                                FirstName = "James",
                                LastName = "T.",
                                Title = "Marketing Manager",
                                Department = "Marketing",
                                Email = "james.smith@example.com",
                                Phone = "5559876543",
                                LinkedInUrl = "https://www.linkedin.com/in/jamessmith",
                                IsVip = false,
                                MemberId = 97
                            },
                            new Contact
                            {
                                Id = 98,
                                FirstName = "Sarah",
                                LastName = "A.",
                                Title = "HR Specialist",
                                Department = "Human Resources",
                                Email = "sarah.johnson@example.com",
                                Phone = "5551122334",
                                LinkedInUrl = "https://www.linkedin.com/in/sarahjohnson",
                                IsVip = false,
                                MemberId = 98
                            },
                            new Contact
                            {
                                Id = 99,
                                FirstName = "DavId",
                                LastName = "L.",
                                Title = "Chief Executive Officer",
                                Department = "Executive",
                                Email = "davId.brown@example.com",
                                Phone = "5552233445",
                                LinkedInUrl = "https://www.linkedin.com/in/davIdbrown",
                                IsVip = true,
                                MemberId = 99
                            },
                            new Contact
                            {
                                Id = 100,
                                FirstName = "Emily",
                                LastName = "M.",
                                Title = "Product Designer",
                                Department = "Design",
                                Email = "emily.williams@example.com",
                                Phone = "5556677889",
                                LinkedInUrl = "https://www.linkedin.com/in/emilywilliams",
                                IsVip = false,
                                MemberId = 100
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
                            new IndustryNAICSCode { Id = 40, MemberId = 40, NAICSCodeId = 2 },
                            new IndustryNAICSCode { Id = 41, MemberId = 41, NAICSCodeId = 1 },  // Member 1, NAICS Code 1
                            new IndustryNAICSCode { Id = 42, MemberId = 42, NAICSCodeId = 2 },  // Member 2, NAICS Code 2
                            new IndustryNAICSCode { Id = 43, MemberId = 43, NAICSCodeId = 3 },  // Member 3, NAICS Code 3
                            new IndustryNAICSCode { Id = 44, MemberId = 44, NAICSCodeId = 4 },  // Member 4, NAICS Code 4
                            new IndustryNAICSCode { Id = 45, MemberId = 45, NAICSCodeId = 5 },  // Member 5, NAICS Code 5
                            new IndustryNAICSCode { Id = 46, MemberId = 46, NAICSCodeId = 6 },  // Member 6, NAICS Code 6
                            new IndustryNAICSCode { Id = 47, MemberId = 47, NAICSCodeId = 7 },  // Member 7, NAICS Code 7
                            new IndustryNAICSCode { Id = 48, MemberId = 48, NAICSCodeId = 8 },  // Member 8, NAICS Code 8
                            new IndustryNAICSCode { Id = 49, MemberId = 49, NAICSCodeId = 9 },  // Member 9, NAICS Code 9
                            new IndustryNAICSCode { Id = 50, MemberId = 50, NAICSCodeId = 10 },  // Member 10, NAICS Code 10
                            new IndustryNAICSCode { Id = 51, MemberId = 51, NAICSCodeId = 11 },  // Member 11, NAICS Code 11
                            new IndustryNAICSCode { Id = 52, MemberId = 52, NAICSCodeId = 12 },  // Member 12, NAICS Code 12
                            new IndustryNAICSCode { Id = 53, MemberId = 53, NAICSCodeId = 13 },  // Member 13, NAICS Code 13
                            new IndustryNAICSCode { Id = 54, MemberId = 54, NAICSCodeId = 14 },  // Member 14, NAICS Code 14
                            new IndustryNAICSCode { Id = 55, MemberId = 55, NAICSCodeId = 15 },  // Member 15, NAICS Code 15
                            new IndustryNAICSCode { Id = 56, MemberId = 56, NAICSCodeId = 16 },  // Member 16, NAICS Code 16
                            new IndustryNAICSCode { Id = 57, MemberId = 57, NAICSCodeId = 17 },  // Member 17, NAICS Code 17
                            new IndustryNAICSCode { Id = 58, MemberId = 58, NAICSCodeId = 18 },  // Member 18, NAICS Code 18
                            new IndustryNAICSCode { Id = 59, MemberId = 59, NAICSCodeId = 19 },  // Member 19, NAICS Code 19
                            new IndustryNAICSCode { Id = 60, MemberId = 60, NAICSCodeId = 1 },  // Member 20, NAICS Code 1
                            new IndustryNAICSCode { Id = 61, MemberId = 61, NAICSCodeId = 2 },  // Member 21, NAICS Code 2
                            new IndustryNAICSCode { Id = 62, MemberId = 62, NAICSCodeId = 3 },  // Member 22, NAICS Code 3
                            new IndustryNAICSCode { Id = 63, MemberId = 63, NAICSCodeId = 4 },  // Member 23, NAICS Code 4
                            new IndustryNAICSCode { Id = 64, MemberId = 64, NAICSCodeId = 5 },  // Member 24, NAICS Code 5
                            new IndustryNAICSCode { Id = 65, MemberId = 65, NAICSCodeId = 6 },  // Member 25, NAICS Code 6
                            new IndustryNAICSCode { Id = 66, MemberId = 66, NAICSCodeId = 7 },  // Member 26, NAICS Code 7
                            new IndustryNAICSCode { Id = 67, MemberId = 67, NAICSCodeId = 8 },  // Member 27, NAICS Code 8
                            new IndustryNAICSCode { Id = 68, MemberId = 68, NAICSCodeId = 9 },  // Member 28, NAICS Code 9
                            new IndustryNAICSCode { Id = 69, MemberId = 69, NAICSCodeId = 10 },  // Member 29, NAICS Code 10
                            new IndustryNAICSCode { Id = 70, MemberId = 70, NAICSCodeId = 11 },  // Member 30, NAICS Code 11
                            new IndustryNAICSCode { Id = 71, MemberId = 71, NAICSCodeId = 12 },  // Member 31, NAICS Code 12
                            new IndustryNAICSCode { Id = 72, MemberId = 72, NAICSCodeId = 13 },  // Member 32, NAICS Code 13
                            new IndustryNAICSCode { Id = 73, MemberId = 73, NAICSCodeId = 14 },  // Member 33, NAICS Code 14
                            new IndustryNAICSCode { Id = 74, MemberId = 74, NAICSCodeId = 15 },  // Member 34, NAICS Code 15
                            new IndustryNAICSCode { Id = 75, MemberId = 75, NAICSCodeId = 16 },  // Member 35, NAICS Code 16
                            new IndustryNAICSCode { Id = 76, MemberId = 76, NAICSCodeId = 17 },  // Member 36, NAICS Code 17
                            new IndustryNAICSCode { Id = 77, MemberId = 77, NAICSCodeId = 18 },  // Member 37, NAICS Code 18
                            new IndustryNAICSCode { Id = 78, MemberId = 78, NAICSCodeId = 19 },  // Member 38, NAICS Code 19
                            new IndustryNAICSCode { Id = 79, MemberId = 79, NAICSCodeId = 1 },  // Member 39, NAICS Code 1
                            new IndustryNAICSCode { Id = 80, MemberId = 80, NAICSCodeId = 2 },
                            new IndustryNAICSCode { Id = 81, MemberId = 81, NAICSCodeId = 2 },  // Member 21, NAICS Code 2
                            new IndustryNAICSCode { Id = 82, MemberId = 82, NAICSCodeId = 3 },  // Member 22, NAICS Code 3
                            new IndustryNAICSCode { Id = 83, MemberId = 83, NAICSCodeId = 4 },  // Member 23, NAICS Code 4
                            new IndustryNAICSCode { Id = 84, MemberId = 84, NAICSCodeId = 5 },  // Member 24, NAICS Code 5
                            new IndustryNAICSCode { Id = 85, MemberId = 85, NAICSCodeId = 6 },  // Member 25, NAICS Code 6
                            new IndustryNAICSCode { Id = 86, MemberId = 86, NAICSCodeId = 7 },  // Member 26, NAICS Code 7
                            new IndustryNAICSCode { Id = 87, MemberId = 87, NAICSCodeId = 8 },  // Member 27, NAICS Code 8
                            new IndustryNAICSCode { Id = 88, MemberId = 88, NAICSCodeId = 9 },  // Member 28, NAICS Code 9
                            new IndustryNAICSCode { Id = 89, MemberId = 89, NAICSCodeId = 10 },  // Member 29, NAICS Code 10
                            new IndustryNAICSCode { Id = 90, MemberId = 90, NAICSCodeId = 11 },  // Member 30, NAICS Code 11
                            new IndustryNAICSCode { Id = 91, MemberId = 91, NAICSCodeId = 12 },  // Member 31, NAICS Code 12
                            new IndustryNAICSCode { Id = 92, MemberId = 92, NAICSCodeId = 13 },  // Member 32, NAICS Code 13
                            new IndustryNAICSCode { Id = 93, MemberId = 93, NAICSCodeId = 14 },  // Member 33, NAICS Code 14
                            new IndustryNAICSCode { Id = 94, MemberId = 94, NAICSCodeId = 15 },  // Member 34, NAICS Code 15
                            new IndustryNAICSCode { Id = 95, MemberId = 95, NAICSCodeId = 16 },  // Member 35, NAICS Code 16
                            new IndustryNAICSCode { Id = 96, MemberId = 96, NAICSCodeId = 17 },  // Member 36, NAICS Code 17
                            new IndustryNAICSCode { Id = 97, MemberId = 97, NAICSCodeId = 18 },  // Member 37, NAICS Code 18
                            new IndustryNAICSCode { Id = 98, MemberId = 98, NAICSCodeId = 19 },  // Member 38, NAICS Code 19
                            new IndustryNAICSCode { Id = 99, MemberId = 99, NAICSCodeId = 1 },  // Member 39, NAICS Code 1
                            new IndustryNAICSCode { Id = 100, MemberId = 100, NAICSCodeId = 2 }// Member 40, NAICS Code 2
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
                        new MemberNote { Id = 40, MemberId = 40, Note = "Discussed CRM's scalability and future-proofing with the client.", CreatedAt = DateTime.Now.AddDays(-9) },
                        new MemberNote { Id = 41, MemberId = 41, Note = "Client inquiry regarding CRM customization options for reporting.", CreatedAt = DateTime.Now.AddDays(-8) },
                        new MemberNote { Id = 42, MemberId = 42, Note = "Initial CRM implementation discussion, defining project scope.", CreatedAt = DateTime.Now.AddDays(-7) },
                        new MemberNote { Id = 43, MemberId = 43, Note = "Follow-up meeting to address client concerns about data integration.", CreatedAt = DateTime.Now.AddDays(-6) },
                        new MemberNote { Id = 44, MemberId = 44, Note = "Demonstration of new CRM features for enhanced data reporting.", CreatedAt = DateTime.Now.AddDays(-5) },
                        new MemberNote { Id = 45, MemberId = 45, Note = "CRM scalability discussion to accommodate the client's growing business.", CreatedAt = DateTime.Now.AddDays(-4) },
                        new MemberNote { Id = 46, MemberId = 46, Note = "Presentation of CRM's mobile app functionalities.", CreatedAt = DateTime.Now.AddDays(-3) },
                        new MemberNote { Id = 47, MemberId = 47, Note = "Client feedback session on CRM's user interface and navigation.", CreatedAt = DateTime.Now.AddDays(-2) },
                        new MemberNote { Id = 48, MemberId = 48, Note = "Assessed the CRM's integration with the client's existing software.", CreatedAt = DateTime.Now.AddDays(-1) },
                        new MemberNote { Id = 49, MemberId = 49, Note = "CRM performance review based on client feedback and usage data.", CreatedAt = DateTime.Now.AddDays(-30) },
                        new MemberNote { Id = 50, MemberId = 50, Note = "Client requested further customization on CRM's reporting tools.", CreatedAt = DateTime.Now.AddDays(-29) },
                        new MemberNote { Id = 51, MemberId = 51, Note = "Follow-up meeting to discuss CRM's impact on operational efficiency.", CreatedAt = DateTime.Now.AddDays(-28) },
                        new MemberNote { Id = 52, MemberId = 52, Note = "Scheduled a training session on CRM's advanced data analytics features.", CreatedAt = DateTime.Now.AddDays(-27) },
                        new MemberNote { Id = 53, MemberId = 53, Note = "Client requests integration with third-party tools for CRM.", CreatedAt = DateTime.Now.AddDays(-26) },
                        new MemberNote { Id = 54, MemberId = 54, Note = "Discussion on enhancing CRM's data security protocols.", CreatedAt = DateTime.Now.AddDays(-25) },
                        new MemberNote { Id = 55, MemberId = 55, Note = "Reviewed CRM performance metrics with the client.", CreatedAt = DateTime.Now.AddDays(-24) },
                        new MemberNote { Id = 56, MemberId = 56, Note = "Client shared feedback on CRM's data privacy features.", CreatedAt = DateTime.Now.AddDays(-23) },
                        new MemberNote { Id = 57, MemberId = 57, Note = "Explained upcoming CRM software updates to the client.", CreatedAt = DateTime.Now.AddDays(-22) },
                        new MemberNote { Id = 58, MemberId = 58, Note = "Final meeting to review CRM's impact and gather client feedback.", CreatedAt = DateTime.Now.AddDays(-21) },
                        new MemberNote { Id = 59, MemberId = 59, Note = "Client requested a demo for advanced CRM user roles and permissions.", CreatedAt = DateTime.Now.AddDays(-20) },
                        new MemberNote { Id = 60, MemberId = 60, Note = "Follow-up on CRM feature requests and prioritization for next update.", CreatedAt = DateTime.Now.AddDays(-19) },
                        new MemberNote { Id = 61, MemberId = 61, Note = "Discussed data integration challenges and next steps with the client.", CreatedAt = DateTime.Now.AddDays(-18) },
                        new MemberNote { Id = 62, MemberId = 62, Note = "Client's feedback on CRM's mobile app interface and usability.", CreatedAt = DateTime.Now.AddDays(-17) },
                        new MemberNote { Id = 63, MemberId = 63, Note = "Clarified CRM's data storage solutions for the client's needs.", CreatedAt = DateTime.Now.AddDays(-16) },
                        new MemberNote { Id = 64, MemberId = 64, Note = "Client inquired about CRM's scalability to handle future growth.", CreatedAt = DateTime.Now.AddDays(-15) },
                        new MemberNote { Id = 65, MemberId = 65, Note = "Reviewed CRM's feature requests and the roadmap for next year.", CreatedAt = DateTime.Now.AddDays(-14) },
                        new MemberNote { Id = 66, MemberId = 66, Note = "Presentation of CRM's new features to senior management.", CreatedAt = DateTime.Now.AddDays(-13) },
                        new MemberNote { Id = 67, MemberId = 67, Note = "Explained CRM's data reporting tools and their benefits to the client.", CreatedAt = DateTime.Now.AddDays(-12) },
                        new MemberNote { Id = 68, MemberId = 68, Note = "Client expressed satisfaction with CRM's customization options.", CreatedAt = DateTime.Now.AddDays(-11) },
                        new MemberNote { Id = 69, MemberId = 69, Note = "Client feedback on CRM's real-time reporting features.", CreatedAt = DateTime.Now.AddDays(-10) },
                        new MemberNote { Id = 70, MemberId = 70, Note = "Discussions on CRM's advanced data analytics capabilities.", CreatedAt = DateTime.Now.AddDays(-9) },
                        new MemberNote { Id = 71, MemberId = 71, Note = "CRM integration discussion with the client's external systems.", CreatedAt = DateTime.Now.AddDays(-8) },
                        new MemberNote { Id = 72, MemberId = 72, Note = "Client requested additional support for CRM deployment.", CreatedAt = DateTime.Now.AddDays(-7) },
                        new MemberNote { Id = 73, MemberId = 73, Note = "Follow-up on client feedback regarding CRM user interface.", CreatedAt = DateTime.Now.AddDays(-6) },
                        new MemberNote { Id = 74, MemberId = 74, Note = "Explained CRM's security features and data protection protocols.", CreatedAt = DateTime.Now.AddDays(-5) },
                        new MemberNote { Id = 75, MemberId = 75, Note = "Final review of CRM system before go-live.", CreatedAt = DateTime.Now.AddDays(-4) },
                        new MemberNote { Id = 76, MemberId = 76, Note = "Client inquired about CRM's future updates and new features.", CreatedAt = DateTime.Now.AddDays(-3) },
                        new MemberNote { Id = 77, MemberId = 77, Note = "Client requested assistance with CRM's third-party integrations.", CreatedAt = DateTime.Now.AddDays(-2) },
                        new MemberNote { Id = 78, MemberId = 78, Note = "CRM performance review and feedback collection from the client.", CreatedAt = DateTime.Now.AddDays(-1) },
                        new MemberNote { Id = 79, MemberId = 79, Note = "Client follow-up on CRM features and system performance.", CreatedAt = DateTime.Now.AddDays(-30) },
                        new MemberNote { Id = 80, MemberId = 80, Note = "Scheduled a meeting to discuss future CRM feature enhancements.", CreatedAt = DateTime.Now.AddDays(-29) },
                        new MemberNote { Id = 81, MemberId = 81, Note = "Client's feedback on CRM's usability and user-friendliness.", CreatedAt = DateTime.Now.AddDays(-28) },
                        new MemberNote { Id = 82, MemberId = 82, Note = "Discussed CRM customization options for enhanced client reporting.", CreatedAt = DateTime.Now.AddDays(-27) },
                        new MemberNote { Id = 83, MemberId = 83, Note = "Client requested new CRM functionalities to improve workflow.", CreatedAt = DateTime.Now.AddDays(-26) },
                        new MemberNote { Id = 84, MemberId = 84, Note = "Meeting to review CRM's features and potential improvements.", CreatedAt = DateTime.Now.AddDays(-25) },
                        new MemberNote { Id = 85, MemberId = 85, Note = "Client requested additional training on CRM's advanced features.", CreatedAt = DateTime.Now.AddDays(-24) },
                        new MemberNote { Id = 86, MemberId = 86, Note = "Follow-up on CRM deployment success and initial feedback.", CreatedAt = DateTime.Now.AddDays(-23) },
                        new MemberNote { Id = 87, MemberId = 87, Note = "Discussion on CRM's data backup and disaster recovery strategies.", CreatedAt = DateTime.Now.AddDays(-22) },
                        new MemberNote { Id = 88, MemberId = 88, Note = "Review of CRM's data reporting accuracy and client feedback.", CreatedAt = DateTime.Now.AddDays(-21) },
                        new MemberNote { Id = 89, MemberId = 89, Note = "Client follow-up to discuss CRM user training and ongoing support.", CreatedAt = DateTime.Now.AddDays(-20) },
                        new MemberNote { Id = 90, MemberId = 90, Note = "Client feedback session on CRM's workflow automation features.", CreatedAt = DateTime.Now.AddDays(-19) },
                        new MemberNote { Id = 91, MemberId = 91, Note = "Discussions on improving CRM's data processing speed.", CreatedAt = DateTime.Now.AddDays(-18) },
                        new MemberNote { Id = 92, MemberId = 92, Note = "Meeting to discuss CRM's future roadmap and client requirements.", CreatedAt = DateTime.Now.AddDays(-17) },
                        new MemberNote { Id = 93, MemberId = 93, Note = "Client requested additional features for CRM's user reporting.", CreatedAt = DateTime.Now.AddDays(-16) },
                        new MemberNote { Id = 94, MemberId = 94, Note = "Follow-up on CRM's post-implementation success and feedback.", CreatedAt = DateTime.Now.AddDays(-15) },
                        new MemberNote { Id = 95, MemberId = 95, Note = "Discussion on the upcoming CRM version and new functionalities.", CreatedAt = DateTime.Now.AddDays(-14) },
                        new MemberNote { Id = 96, MemberId = 96, Note = "Explained CRM's new security features and improvements.", CreatedAt = DateTime.Now.AddDays(-13) },
                        new MemberNote { Id = 97, MemberId = 97, Note = "Client requested CRM customization to better fit their reporting needs.", CreatedAt = DateTime.Now.AddDays(-12) },
                        new MemberNote { Id = 98, MemberId = 98, Note = "Scheduled follow-up to review CRM's impact on client workflow.", CreatedAt = DateTime.Now.AddDays(-11) },
                        new MemberNote { Id = 99, MemberId = 99, Note = "Explained CRM's data encryption and privacy policies to the client.", CreatedAt = DateTime.Now.AddDays(-10) },
                        new MemberNote { Id = 100, MemberId = 100, Note = "Final meeting to discuss CRM's performance and next steps.", CreatedAt = DateTime.Now.AddDays(-9) }
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
