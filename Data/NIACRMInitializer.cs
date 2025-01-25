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
                    if (!context.Industries.Any())
                    {
                        context.Industries.AddRange(
                            new Industry { ID = 1, IndustryName = "Alpha Steel", IndustrySize = 250 },
                            new Industry { ID = 2, IndustryName = "TISCO CO.", IndustrySize = 150 },
                            new Industry { ID = 3, IndustryName = "M Time Irons", IndustrySize = 100 },
                            new Industry { ID = 4, IndustryName = "Forge & Foundry Inc.", IndustrySize = 300 },
                            new Industry { ID = 5, IndustryName = "Northern Metalworks", IndustrySize = 120 },
                            new Industry { ID = 6, IndustryName = "Titanium Solutions", IndustrySize = 400 },
                            new Industry { ID = 7, IndustryName = "Phoenix Alloys", IndustrySize = 350 },
                            new Industry { ID = 8, IndustryName = "Galaxy Metals", IndustrySize = 500 },
                            new Industry { ID = 9, IndustryName = "Ironclad Industries", IndustrySize = 220 },
                            new Industry { ID = 10, IndustryName = "Silverline Fabrication", IndustrySize = 180 },
                            new Industry { ID = 11, IndustryName = "Star Steelworks", IndustrySize = 230 },
                            new Industry { ID = 12, IndustryName = "Summit Metal Co.", IndustrySize = 270 },
                            new Industry { ID = 13, IndustryName = "Everest Iron Corp.", IndustrySize = 210 },
                            new Industry { ID = 14, IndustryName = "Prime Alloy Coatings", IndustrySize = 160 },
                            new Industry { ID = 15, IndustryName = "Magnum Steel Solutions", IndustrySize = 190 },
                            new Industry { ID = 16, IndustryName = "Quantum Tech Innovations", IndustrySize = 450 },
                            new Industry { ID = 17, IndustryName = "Aurora Renewable Energy", IndustrySize = 500 },
                            new Industry { ID = 18, IndustryName = "Vertex Financial Group", IndustrySize = 80 },
                            new Industry { ID = 19, IndustryName = "Nova Biotech Labs", IndustrySize = 60 },
                            new Industry { ID = 20, IndustryName = "Summit Construction Co.", IndustrySize = 250 },
                            new Industry { ID = 21, IndustryName = "Oceanic Shipping Corp", IndustrySize = 600 },
                            new Industry { ID = 22, IndustryName = "Evergreen Agriculture", IndustrySize = 550 },
                            new Industry { ID = 23, IndustryName = "Ironclad Manufacturing Ltd.", IndustrySize = 300 },
                            new Industry { ID = 24, IndustryName = "Skyline Architects Inc.", IndustrySize = 130 },
                            new Industry { ID = 25, IndustryName = "Pinnacle Consulting Services", IndustrySize = 90 },
                            new Industry { ID = 26, IndustryName = "Crystal Water Solutions", IndustrySize = 110 },
                            new Industry { ID = 27, IndustryName = "Elite Healthcare Partners", IndustrySize = 150 },
                            new Industry { ID = 28, IndustryName = "Galaxy IT Solutions", IndustrySize = 400 },
                            new Industry { ID = 29, IndustryName = "Urban Infrastructure Group", IndustrySize = 350 },
                            new Industry { ID = 30, IndustryName = "Horizon Aerospace Inc.", IndustrySize = 450 },
                            new Industry { ID = 31, IndustryName = "Cobalt Mining Ventures", IndustrySize = 500 },
                            new Industry { ID = 32, IndustryName = "LakesIde Resorts and Hotels", IndustrySize = 200 },
                            new Industry { ID = 33, IndustryName = "NextGen Media Productions", IndustrySize = 100 },
                            new Industry { ID = 34, IndustryName = "Crestwood Pharmaceutical", IndustrySize = 120 },
                            new Industry { ID = 35, IndustryName = "Dynamic Logistics Group", IndustrySize = 180 },
                            new Industry { ID = 36, IndustryName = "Northern Timber Products", IndustrySize = 160 },
                            new Industry { ID = 37, IndustryName = "Brightline Education Systems", IndustrySize = 50 },
                            new Industry { ID = 38, IndustryName = "Fusion Energy Solutions", IndustrySize = 300 },
                            new Industry { ID = 39, IndustryName = "Trailblazer Automotive Group", IndustrySize = 450 },
                            new Industry { ID = 40, IndustryName = "Harvest Foods International", IndustrySize = 400 },
                            new Industry { ID = 41, IndustryName = "Regal Entertainment Network", IndustrySize = 220 },
                            new Industry { ID = 42, IndustryName = "EcoSmart Waste Management", IndustrySize = 270 },
                            new Industry { ID = 43, IndustryName = "Summit Legal Services", IndustrySize = 130 },
                            new Industry { ID = 44, IndustryName = "Zenith Apparel Ltd.", IndustrySize = 80 },
                            new Industry { ID = 45, IndustryName = "BlueWave Software Inc.", IndustrySize = 200 }

                        );
                        context.SaveChanges();
                    }

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
                            MemberFirstName = "John",
                            MemberMiddleName = null,
                            MemberLastName = "Doe",
                            JoinDate = new DateTime(2021, 6, 15),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>  // Correctly initialize Addresses as a collection
                            {
                                new Address
                                {
                                    AddressLine1 = "123 Elm St",
                                    AddressLine2 = "Apt 4B",
                                    City = "Metaltown",
                                    StateProvince = "Metal State",
                                    PostalCode = "M1E2L3",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 2,
                            MemberFirstName = "Jane",
                            MemberMiddleName= "Marie",
                            MemberLastName = "Smith",
                            JoinDate = new DateTime(2022, 1, 20),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>  // Initialize as a list
                            {
                                new Address
                                {
                                    AddressLine1 = "456 Oak Ave",
                                    AddressLine2 = null,
                                    City = "Ironville",
                                    StateProvince = "Steel Province",
                                    PostalCode = "I2R3O4",
                                    Country = "Canada"
                                }
                            }


                        },
                        new Member
                        {
                            ID = 3,
                            MemberFirstName = "Robert",
                            MemberMiddleName = "Joe",
                            MemberLastName = "Johnson",
                            JoinDate = new DateTime(2023, 3, 12),
                            StandingStatus = StandingStatus.Cancelled,
                            Addresses = new List<Address>  // Initialize as a list
                            {
                                new Address
                                {
                                    AddressLine1 = "789 Pine Rd",
                                    AddressLine2 = "Suite 302",
                                    City = "Steelton",
                                    StateProvince = "Metal Zone",
                                    PostalCode = "S3T4E5",
                                    Country = "UK"
                                }
                            }

                        },
                        new Member
                        {
                            ID = 4,
                            MemberFirstName = "Emily",
                            MemberMiddleName= "Sharma",
                            MemberLastName = "Davis",
                            JoinDate = new DateTime(2022, 8, 8),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>  // Initialize as a list
                            {
                                new Address
                                {
                                    AddressLine1 = "789 Pine Rd",  // Corrected property name as per your requirement
                                    AddressLine2 = "Suite 302",   // Corrected property name as per your requirement
                                    City = "Steelton",
                                    StateProvince = "Metal Zone",
                                    PostalCode = "S3T4E5",
                                    Country = "UK"
                                }
                            }

                        },
                        new Member
                        {
                            ID = 5,
                            MemberFirstName = "Michael",
                            MemberMiddleName = null ,
                            MemberLastName = "Brown",
                            JoinDate = new DateTime(2020, 5, 10),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>  // Initialize as a list
                            {
                                new Address
                                {
                                    AddressLine1 = "201 Maple Street",  // Correct property name
                                    AddressLine2 = "Unit 101",          // Correct property name
                                    City = "Copperville",
                                    StateProvince = "Metalland",
                                    PostalCode = "C2P1E1",
                                    Country = "USA"
                                }
                            }

                        },
                        new Member
                        {
                            ID = 6,
                            MemberFirstName = "Sarah",
                            MemberMiddleName= "Singh",
                            MemberLastName = "Johnson",
                            JoinDate = new DateTime(2021, 9, 15),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>  // Initialize as a list
                            {
                                new Address
                                {
                                    AddressLine1 = "543 Cedar Lane",  // Correct property name
                                    AddressLine2 = null,              // Correct property name
                                    City = "Ironcrest",
                                    StateProvince = "Steelstate",
                                    PostalCode = "I3N4O5",
                                    Country = "Canada"
                                }
                            }

                        },
                        new Member
                        {
                            ID = 7,
                            MemberFirstName = "William",
                            MemberMiddleName = "Pat",
                            MemberLastName = "Taylor",
                            JoinDate = new DateTime(2019, 4, 20),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>  // Initialize as a list
                            {
                                new Address
                                {
                                    AddressLine1 = "15 Granite Road",  // Correct property name
                                    AddressLine2 = "Suite 205",       // Correct property name
                                    City = "Minerstown",
                                    StateProvince = "Ore County",
                                    PostalCode = "G1R2A8",
                                    Country = "UK"
                                }
                            }

                        },
                        new Member
                        {
                            ID = 8,
                            MemberFirstName = "Jessica",
                            MemberMiddleName = "Divyansh",
                            MemberLastName = "Martinez",
                            JoinDate = new DateTime(2023, 1, 1),
                            StandingStatus = StandingStatus.Cancelled,
                            Addresses = new List<Address>  // Initialize as a list
                            {
                                new Address
                                {
                                    AddressLine1 = "876 Redwood Drive",  // Correct property name
                                    AddressLine2 = null,                 // Correct property name
                                    City = "Steelville",
                                    StateProvince = "Forge State",
                                    PostalCode = "R4D6W2",
                                    Country = "USA"
                                }
                            }

                        },
                        new Member
                        {
                            ID = 9,
                            MemberFirstName = "James",
                            MemberMiddleName = "Hosi",
                            MemberLastName = "Lee",
                            JoinDate = new DateTime(2022, 6, 30),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>  // Initialize as a list
                            {
                                new Address
                                {
                                    AddressLine1 = "654 Willow Avenue",  // Correct property name
                                    AddressLine2 = "Floor 3",            // Correct property name
                                    City = "Ironworks",
                                    StateProvince = "Steel Kingdom",
                                    PostalCode = "W2L5A7",
                                    Country = "Australia"
                                }
                            }

                        },
                        new Member
                        {
                            ID = 10,
                            MemberFirstName = "Olivia",
                            MemberMiddleName = null,
                            MemberLastName = "Rody",
                            JoinDate = new DateTime(2020, 2, 25),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>  // Initialize as a list
                            {
                                new Address
                                {
                                    AddressLine1 = "99 Ash Boulevard",
                                    AddressLine2 = "Building A",
                                    City = "Metaltown",
                                    StateProvince = "Steel Nation",
                                    PostalCode = "A9H4L2",
                                    Country = "Canada"
                                }
                            }

                        },
                        new Member
                        {
                            ID = 11,
                            MemberFirstName = "Liam",
                            MemberMiddleName = null,
                            MemberLastName = "Smith",
                            JoinDate = new DateTime(2021, 3, 17),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "12 Maple Street",
                                    AddressLine2 = "Suite 101",
                                    City = "Oakwood",
                                    StateProvince = "Green Valley",
                                    PostalCode = "G7F8M3",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 12,
                            MemberFirstName = "Emma",
                            MemberMiddleName = "Grace",
                            MemberLastName = "Johnson",
                            JoinDate = new DateTime(2022, 1, 5),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "45 Pine Road",
                                    AddressLine2 = "Apt 12B",
                                    City = "Springfield",
                                    StateProvince = "Illinois",
                                    PostalCode = "62704",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 13,
                            MemberFirstName = "Ava",
                            MemberMiddleName = "Marie",
                            MemberLastName = "Williams",
                            JoinDate = new DateTime(2019, 8, 22),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "99 Oak Drive",
                                    AddressLine2 = "Floor 3",
                                    City = "Rivertown",
                                    StateProvince = "Ocean State",
                                    PostalCode = "O4R2T1",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 14,
                            MemberFirstName = "Noah",
                            MemberMiddleName = "Alexander",
                            MemberLastName = "Brown",
                            JoinDate = new DateTime(2020, 6, 10),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "120 Birch Lane",
                                    AddressLine2 = "Unit 205",
                                    City = "Hillcrest",
                                    StateProvince = "Mountain Region",
                                    PostalCode = "M2N1K7",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 15,
                            MemberFirstName = "Sophia",
                            MemberMiddleName = null,
                            MemberLastName = "Davis",
                            JoinDate = new DateTime(2021, 4, 30),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "58 Cedar Street",
                                    AddressLine2 = "Building 4",
                                    City = "Brookside",
                                    StateProvince = "Blue Ridge",
                                    PostalCode = "B1A3N8",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 16,
                            MemberFirstName = "James",
                            MemberMiddleName = "Edward",
                            MemberLastName = "Martinez",
                            JoinDate = new DateTime(2018, 9, 15),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "111 Cherry Avenue",
                                    AddressLine2 = "Suite 500",
                                    City = "Silverbrook",
                                    StateProvince = "Sunshine State",
                                    PostalCode = "S2F9H1",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 17,
                            MemberFirstName = "Isabella",
                            MemberMiddleName = "Rose",
                            MemberLastName = "Taylor",
                            JoinDate = new DateTime(2021, 11, 9),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "78 Elm Street",
                                    AddressLine2 = "Floor 2",
                                    City = "Brighton",
                                    StateProvince = "New Horizons",
                                    PostalCode = "N5W2B4",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 18,
                            MemberFirstName = "Mason",
                            MemberMiddleName = "Joshua",
                            MemberLastName = "Anderson",
                            JoinDate = new DateTime(2022, 7, 21),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "21 Fir Street",
                                    AddressLine2 = "Unit 50",
                                    City = "New Haven",
                                    StateProvince = "Mystic Valley",
                                    PostalCode = "M3H8G2",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 19,
                            MemberFirstName = "Amelia",
                            MemberMiddleName = "Lynn",
                            MemberLastName = "Clark",
                            JoinDate = new DateTime(2021, 12, 18),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "36 Redwood Crescent",
                                    AddressLine2 = "Apartment 3",
                                    City = "Clearwater",
                                    StateProvince = "River Valley",
                                    PostalCode = "C8D2T5",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 20,
                            MemberFirstName = "Ethan",
                            MemberMiddleName = "Michael",
                            MemberLastName = "Garcia",
                            JoinDate = new DateTime(2020, 12, 14),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "64 Aspen Road",
                                    AddressLine2 = "Unit 207",
                                    City = "Forest Grove",
                                    StateProvince = "Woodland Hills",
                                    PostalCode = "W5T6D2",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 21,
                            MemberFirstName = "Charlotte",
                            MemberMiddleName = "Anne",
                            MemberLastName = "Hernandez",
                            JoinDate = new DateTime(2023, 2, 1),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "77 Riverbend Street",
                                    AddressLine2 = "Suite 9",
                                    City = "Silver Valley",
                                    StateProvince = "Sunset Coast",
                                    PostalCode = "S8K9J4",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 22,
                            MemberFirstName = "Benjamin",
                            MemberMiddleName = "David",
                            MemberLastName = "Moore",
                            JoinDate = new DateTime(2019, 5, 3),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "90 Willow Drive",
                                    AddressLine2 = "Floor 1",
                                    City = "Lakeside",
                                    StateProvince = "Silverwood",
                                    PostalCode = "L6C7S9",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 23,
                            MemberFirstName = "Ella",
                            MemberMiddleName = "Sophia",
                            MemberLastName = "Scott",
                            JoinDate = new DateTime(2020, 11, 19),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "102 Maple Lane",
                                    AddressLine2 = "Apt 1003",
                                    City = "Hill Valley",
                                    StateProvince = "Autumn Ridge",
                                    PostalCode = "A7X5Z2",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 24,
                            MemberFirstName = "Jack",
                            MemberMiddleName = null,
                            MemberLastName = "King",
                            JoinDate = new DateTime(2021, 8, 23),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "33 Ocean Breeze",
                                    AddressLine2 = "Unit 3",
                                    City = "Sunny Bay",
                                    StateProvince = "Tropical State",
                                    PostalCode = "T5Q1E2",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 25,
                            MemberFirstName = "Zoe",
                            MemberMiddleName = "Grace",
                            MemberLastName = "Lee",
                            JoinDate = new DateTime(2022, 9, 8),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "134 Rosewood Road",
                                    AddressLine2 = "Suite 101",
                                    City = "Lakeshore",
                                    StateProvince = "Northern Territory",
                                    PostalCode = "L8V2S3",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 26,
                            MemberFirstName = "Lucas",
                            MemberMiddleName = "Nathaniel",
                            MemberLastName = "Perez",
                            JoinDate = new DateTime(2021, 10, 5),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "56 Sunset Boulevard",
                                    AddressLine2 = "Building 2",
                                    City = "Greenridge",
                                    StateProvince = "Hill Valley",
                                    PostalCode = "G9T2R5",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 27,
                            MemberFirstName = "Mia",
                            MemberMiddleName = "Isabel",
                            MemberLastName = "Gonzalez",
                            JoinDate = new DateTime(2022, 2, 28),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "22 Brook Street",
                                    AddressLine2 = "Floor 5",
                                    City = "Sunrise City",
                                    StateProvince = "East Valley",
                                    PostalCode = "B3F6M2",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 28,
                            MemberFirstName = "Henry",
                            MemberMiddleName = "Charles",
                            MemberLastName = "Martinez",
                            JoinDate = new DateTime(2018, 11, 1),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "72 Birchwood Drive",
                                    AddressLine2 = "Apt 9B",
                                    City = "Riverwood",
                                    StateProvince = "Greenwood",
                                    PostalCode = "G1H8K3",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 29,
                            MemberFirstName = "Grace",
                            MemberMiddleName = "Lily",
                            MemberLastName = "Nguyen",
                            JoinDate = new DateTime(2022, 4, 19),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "23 Forest Avenue",
                                    AddressLine2 = "Unit 12",
                                    City = "Autumn Park",
                                    StateProvince = "Misty Hills",
                                    PostalCode = "F9D8P6",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 30,
                            MemberFirstName = "Daniel",
                            MemberMiddleName = "Jesse",
                            MemberLastName = "Harris",
                            JoinDate = new DateTime(2023, 1, 7),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "123 Oakwood Drive",
                                    AddressLine2 = "Apt 406",
                                    City = "Maple Town",
                                    StateProvince = "Riverdale",
                                    PostalCode = "M1H2N4",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 31,
                            MemberFirstName = "Sophie",
                            MemberMiddleName = "Claire",
                            MemberLastName = "Jackson",
                            JoinDate = new DateTime(2021, 5, 30),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "10 Willow Street",
                                    AddressLine2 = "Suite 10",
                                    City = "Starwood",
                                    StateProvince = "Golden Hills",
                                    PostalCode = "S1A7Q5",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 32,
                            MemberFirstName = "David",
                            MemberMiddleName = "Victor",
                            MemberLastName = "Roberts",
                            JoinDate = new DateTime(2019, 7, 23),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "58 Chestnut Lane",
                                    AddressLine2 = "Unit 4",
                                    City = "Pinehill",
                                    StateProvince = "Silverwood",
                                    PostalCode = "P7A6V9",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 33,
                            MemberFirstName = "Lily",
                            MemberMiddleName = null,
                            MemberLastName = "Adams",
                            JoinDate = new DateTime(2023, 5, 14),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "45 Sunset Way",
                                    AddressLine2 = "Building 6",
                                    City = "Crystal Falls",
                                    StateProvince = "Mountain Ridge",
                                    PostalCode = "C5H1N2",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 34,
                            MemberFirstName = "Jack",
                            MemberMiddleName = "Elliott",
                            MemberLastName = "Sanchez",
                            JoinDate = new DateTime(2021, 3, 12),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "89 Mountain Avenue",
                                    AddressLine2 = "Suite 301",
                                    City = "Rockport",
                                    StateProvince = "Redwood Hills",
                                    PostalCode = "R7P8X3",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 35,
                            MemberFirstName = "Natalie",
                            MemberMiddleName = null,
                            MemberLastName = "Mitchell",
                            JoinDate = new DateTime(2022, 6, 20),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "65 Pine Hill",
                                    AddressLine2 = "Apt 203",
                                    City = "Mapleton",
                                    StateProvince = "Sunny Fields",
                                    PostalCode = "M3L8W9",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 36,
                            MemberFirstName = "Ella",
                            MemberMiddleName = "Mae",
                            MemberLastName = "Kim",
                            JoinDate = new DateTime(2021, 6, 17),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "53 Bluebell Road",
                                    AddressLine2 = "Floor 4",
                                    City = "Riverside",
                                    StateProvince = "Green Hills",
                                    PostalCode = "B2F6T4",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 37,
                            MemberFirstName = "David",
                            MemberMiddleName = null,
                            MemberLastName = "Baker",
                            JoinDate = new DateTime(2019, 12, 5),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "31 Maple Terrace",
                                    AddressLine2 = "Unit 203",
                                    City = "Riverfield",
                                    StateProvince = "North Hills",
                                    PostalCode = "N8Y5D7",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 38,
                            MemberFirstName = "Charlotte",
                            MemberMiddleName = "Daisy",
                            MemberLastName = "Foster",
                            JoinDate = new DateTime(2020, 7, 1),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "77 Birchwood Crescent",
                                    AddressLine2 = "Apt 306",
                                    City = "Lakeview",
                                    StateProvince = "Horizon Bay",
                                    PostalCode = "L3D4S8",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 39,
                            MemberFirstName = "Michael",
                            MemberMiddleName = "Harrison",
                            MemberLastName = "Evans",
                            JoinDate = new DateTime(2022, 3, 29),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "16 Redberry Lane",
                                    AddressLine2 = "Unit 4",
                                    City = "Silver Ridge",
                                    StateProvince = "Western Province",
                                    PostalCode = "S4G5A2",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 40,
                            MemberFirstName = "Elena",
                            MemberMiddleName = "Paige",
                            MemberLastName = "Graham",
                            JoinDate = new DateTime(2021, 9, 25),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "19 Golden Court",
                                    AddressLine2 = "Suite 601",
                                    City = "Mountain Creek",
                                    StateProvince = "Silverstone",
                                    PostalCode = "G3J2B1",
                                    Country = "USA"
                                }
                            }
                        }

                        );
                        context.SaveChanges();
                    }
                    if (!context.MemberIndustries.Any())
                    {
                        context.MemberIndustries.AddRange(
                            new MemberIndustry {MemberId = 1, IndustryId = 1 },
                            new MemberIndustry {MemberId = 2, IndustryId = 2 },
                            new MemberIndustry {MemberId = 3, IndustryId = 3 },
                            new MemberIndustry {MemberId = 4, IndustryId = 4 },
                            new MemberIndustry {MemberId = 5, IndustryId = 5 },
                            new MemberIndustry {MemberId = 6, IndustryId = 6 },
                            new MemberIndustry {MemberId = 7, IndustryId = 7 },
                            new MemberIndustry {MemberId = 8, IndustryId = 8 },
                            new MemberIndustry {MemberId = 9, IndustryId = 9 },
                            new MemberIndustry {MemberId = 10, IndustryId = 10 },
                            new MemberIndustry {MemberId = 10, IndustryId = 41 },
                            new MemberIndustry {MemberId = 11, IndustryId = 11 },
                            new MemberIndustry {MemberId = 12, IndustryId = 12 },
                            new MemberIndustry {MemberId = 13, IndustryId = 13 },
                            new MemberIndustry {MemberId = 14, IndustryId = 14 },
                            new MemberIndustry {MemberId = 15, IndustryId = 15 },
                            new MemberIndustry {MemberId = 15, IndustryId = 42 },
                            new MemberIndustry {MemberId = 16, IndustryId = 16 },
                            new MemberIndustry {MemberId = 17, IndustryId = 17 },
                            new MemberIndustry {MemberId = 18, IndustryId = 18 },
                            new MemberIndustry {MemberId = 19, IndustryId = 19 },
                            new MemberIndustry {MemberId = 20, IndustryId = 20 },
                            new MemberIndustry {MemberId = 21, IndustryId = 21 },
                            new MemberIndustry {MemberId = 22, IndustryId = 22 },
                            new MemberIndustry {MemberId = 23, IndustryId = 23 },
                            new MemberIndustry {MemberId = 24, IndustryId = 24 },
                            new MemberIndustry {MemberId = 25, IndustryId = 25 },
                            new MemberIndustry {MemberId = 25, IndustryId = 43 },
                            new MemberIndustry {MemberId = 26, IndustryId = 26 },
                            new MemberIndustry {MemberId = 27, IndustryId = 27 },
                            new MemberIndustry {MemberId = 28, IndustryId = 28 },
                            new MemberIndustry {MemberId = 29, IndustryId = 29 },
                            new MemberIndustry {MemberId = 30, IndustryId = 30 },
                            new MemberIndustry {MemberId = 31, IndustryId = 31 },
                            new MemberIndustry {MemberId = 32, IndustryId = 32 },
                            new MemberIndustry {MemberId = 32, IndustryId = 44 },
                            new MemberIndustry {MemberId = 33, IndustryId = 33 },
                            new MemberIndustry {MemberId = 34, IndustryId = 34 },
                            new MemberIndustry {MemberId = 35, IndustryId = 35 },
                            new MemberIndustry {MemberId = 36, IndustryId = 36 },
                            new MemberIndustry {MemberId = 37, IndustryId = 37 },
                            new MemberIndustry {MemberId = 38, IndustryId = 38 },
                            new MemberIndustry {MemberId = 39, IndustryId = 39 },
                            new MemberIndustry {MemberId = 40, IndustryId = 40 },
                            new MemberIndustry {MemberId = 40, IndustryId = 45 }
                            );
                    
                        context.SaveChanges();
                    }
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
                               MemberId = 1
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
                               MemberId = 1
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
                               MemberId = 2
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
                               MemberId = 3
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
                               MemberId = 4
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
                               MemberId = 5
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
                               MemberId = 6
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
                               MemberId = 7
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
                               MemberId = 8
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
                               MemberId = 9
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
                               MemberId = 10
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
                               MemberId = 1
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
                               MemberId = 2
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
                               MemberId = 3
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
                           MemberId = 4
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
                           MemberId = 5
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
                           MemberId = 6
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
                           MemberId = 7
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
                           MemberId = 8
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
                           MemberId = 9
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
                           MemberId = 10
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
                           MemberId = 11
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
                           MemberId = 12
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
                           MemberId = 13
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
                           MemberId = 14
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
                           MemberId = 15
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
                           MemberId = 16
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
                           MemberId = 17
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
                           MemberId = 18
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
                           MemberId = 19
                        }

                        );
                        context.SaveChanges();
                    }


                    //if (!context.Opportunities.Any())
                    //{
                    //    context.Opportunities.AddRange(
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "New Partnership with TechCo",
                    //            OpportunityDescr = "Potential collaboration with TechCo to offer joint solutions.",
                    //            OpportunityStatus = OpportunityStatus.Open,
                    //            ID = 1,  // Assuming Organization with Id 1 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 1, 5),
                    //            //    InteractionNotes = "Initial discussion on potential partnership."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Software Development for HealthCorp",
                    //            OpportunityDescr = "Software development project for HealthCorp to enhance their internal systems.",
                    //            OpportunityStatus = OpportunityStatus.InProgress,
                    //            ID = 2,  // Assuming Organization with Id 2 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 1, 10),
                    //            //    InteractionNotes = "Meeting to finalize project requirements."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Website Overhaul for FinServe",
                    //            OpportunityDescr = "Website redesign project for FinServe to improve their online presence.",
                    //            OpportunityStatus = OpportunityStatus.Closed,
                    //            ID = 3,  // Assuming Organization with Id 3 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 1, 15),
                    //            //    InteractionNotes = "Final meeting to close project details."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Mobile App Development for EduTech",
                    //            OpportunityDescr = "Development of a mobile app for EduTech to expand their reach in the education sector.",
                    //            OpportunityStatus = OpportunityStatus.Open,
                    //            ID = 4,  // Assuming Organization with Id 4 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 1, 18),
                    //            //    InteractionNotes = "Kickoff meeting for mobile app project."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "CRM System for SalesForce",
                    //            OpportunityDescr = "Implementation of a CRM system for SalesForce to improve their customer relationship management.",
                    //            OpportunityStatus = OpportunityStatus.InProgress,
                    //            ID = 5,  // Assuming Organization with Id 5 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 1, 20),
                    //            //    InteractionNotes = "Meeting to discuss CRM system features."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "E-commerce Platform for ShopMart",
                    //            OpportunityDescr = "Development of a full-fledged e-commerce platform for ShopMart.",
                    //            OpportunityStatus = OpportunityStatus.Closed,
                    //            ID = 6,  // Assuming Organization with Id 6 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 1, 22),
                    //            //    InteractionNotes = "Final review meeting before project closure."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "AI Integration for SmartTech",
                    //            OpportunityDescr = "Integration of AI-based solutions for SmartTech's systems.",
                    //            OpportunityStatus = OpportunityStatus.Open,
                    //            ID = 7,  // Assuming Organization with Id 7 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 1, 25),
                    //            //    InteractionNotes = "Discussion about AI integration and scope."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Cloud Migration for DataCloud",
                    //            OpportunityDescr = "Cloud migration for DataCloud to streamline their operations and storage.",
                    //            OpportunityStatus = OpportunityStatus.InProgress,
                    //            ID = 8,  // Assuming Organization with Id 8 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 1, 28),
                    //            //    InteractionNotes = "Discussing cloud architecture for migration."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Cybersecurity for SecureNet",
                    //            OpportunityDescr = "Cybersecurity services for SecureNet to enhance their data protection.",
                    //            OpportunityStatus = OpportunityStatus.Closed,
                    //            ID = 9,  // Assuming Organization with Id 9 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 1, 30),
                    //            //    InteractionNotes = "Final agreement on cybersecurity solutions."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Data Analytics for AnalyticsPro",
                    //            OpportunityDescr = "Implementing a data analytics platform for AnalyticsPro to improve decision-making.",
                    //            OpportunityStatus = OpportunityStatus.Open,
                    //            ID = 10,  // Assuming Organization with Id 10 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 2, 2),
                    //            //    InteractionNotes = "Initial discussion on data analytics requirements."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Business Intelligence for BizIntel",
                    //            OpportunityDescr = "ProvIding business intelligence solutions for BizIntel to enhance reporting capabilities.",
                    //            OpportunityStatus = OpportunityStatus.InProgress,
                    //            ID = 11,  // Assuming Organization with Id 11 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 2, 5),
                    //            //    InteractionNotes = "Meeting to review BI system features."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "ERP System for GlobalCorp",
                    //            OpportunityDescr = "ERP system implementation for GlobalCorp to streamline operations.",
                    //            OpportunityStatus = OpportunityStatus.Closed,
                    //            ID = 12,  // Assuming Organization with Id 12 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 2, 7),
                    //            //    InteractionNotes = "Reviewing implementation plan and timeline."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Virtual Events Platform for EventPro",
                    //            OpportunityDescr = "Development of a platform for virtual events for EventPro.",
                    //            OpportunityStatus = OpportunityStatus.Open,
                    //            ID = 13,  // Assuming Organization with Id 13 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 2, 10),
                    //            //    InteractionNotes = "Kickoff meeting to discuss platform features."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Logistics Management System for MoveIt",
                    //            OpportunityDescr = "Logistics management software for MoveIt to optimize their operations.",
                    //            OpportunityStatus = OpportunityStatus.InProgress,
                    //            ID = 14,  // Assuming Organization with Id 14 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 2, 12),
                    //            //    InteractionNotes = "Discussing project scope and logistics software needs."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Blockchain Solutions for ChainTech",
                    //            OpportunityDescr = "Developing blockchain-based solutions for ChainTech's supply chain management.",
                    //            OpportunityStatus = OpportunityStatus.Closed,
                    //            ID = 15,  // Assuming Organization with Id 15 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 2, 14),
                    //            //    InteractionNotes = "Final meeting on blockchain integration."
                    //            //}
                    //        }
                    //    );
                    //    context.SaveChanges();
                    //}

                    //if (!context.Interactions.Any())
                    //{
                    //    context.Interactions.AddRange(
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 1),
                    //            InteractionNotes = "Initial contact for potential collaboration.",
                    //            ContactId = 1,
                    //           MemberId = 1,
                    //            OpportunityId = 1
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 3),
                    //            InteractionNotes = "Follow-up Email regarding partnership details.",
                    //            ContactId = 2,
                    //           MemberId = 2,
                    //            OpportunityId = 2
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 5),
                    //            InteractionNotes = "Scheduled call to discuss project needs.",
                    //            ContactId = 3,
                    //           MemberId = 3,
                    //            OpportunityId = 3
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 10),
                    //            InteractionNotes = "Met to discuss contract terms and conditions.",
                    //            ContactId = 4,
                    //           MemberId = 4,
                    //            OpportunityId = 4
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 12),
                    //            InteractionNotes = "Client inquiry on pricing models.",
                    //            ContactId = 5,
                    //           MemberId = 1,
                    //            OpportunityId = 5
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 15),
                    //            InteractionNotes = "Discussed solution packages for enterprise clients.",
                    //            ContactId = 6,
                    //           MemberId = 2,
                    //            OpportunityId = 6
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 18),
                    //            InteractionNotes = "Following up on service proposal.",
                    //            ContactId = 7,
                    //           MemberId = 3,
                    //            OpportunityId = 7
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 20),
                    //            InteractionNotes = "Finalizing service agreement terms.",
                    //            ContactId = 8,
                    //           MemberId = 4,
                    //            OpportunityId = 8
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 22),
                    //            InteractionNotes = "Agreement on next steps and deliverables.",
                    //            ContactId = 9,
                    //           MemberId = 1,
                    //            OpportunityId = 9
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 25),
                    //            InteractionNotes = "Reviewing deliverables for upcoming project.",
                    //            ContactId = 10,
                    //           MemberId = 2,
                    //            OpportunityId = 10
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 28),
                    //            InteractionNotes = "Update on progress and timeline.",
                    //            ContactId = 11,
                    //           MemberId = 3,
                    //            OpportunityId = 11
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 30),
                    //            InteractionNotes = "Follow-up on final proposal details.",
                    //            ContactId = 12,
                    //           MemberId = 4,
                    //            OpportunityId = 12
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 2, 2),
                    //            InteractionNotes = "Final meeting before project launch.",
                    //            ContactId = 13,
                    //           MemberId = 1,
                    //            OpportunityId = 13
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 2, 5),
                    //            InteractionNotes = "Confirming details of service agreement.",
                    //            ContactId = 14,
                    //           MemberId = 2,
                    //            OpportunityId = 14
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 2, 7),
                    //            InteractionNotes = "Meeting to finalize documentation.",
                    //            ContactId = 3,
                    //           MemberId = 3,
                    //            OpportunityId = 15
                    //        }
                    //    );
                    //    context.SaveChanges();
                    //}
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

                        //Seed data needed for production and during development

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


                        //new MemberNote
                        //{
                        //    Id = 1,
                        //    MemberId = 1,
                        //    Note = "Initial onboarding discussion with the client about the new CRM implementation project.",
                        //    CreatedAt = DateTime.Now.AddDays(-30)
                        //},
                        //    new MemberNote
                        //    {
                        //        Id = 2,
                        //        MemberId = 2,
                        //        Note = "Client follow-up meeting to discuss the status of the CRM project.",
                        //        CreatedAt = DateTime.Now.AddDays(-20)
                        //    },
                        //    new MemberNote
                        //    {
                        //        Id = 3,
                        //        MemberId = 3,
                        //        Note = "Reviewed the feedback provided by the client regarding the CRM customization.",
                        //        CreatedAt = DateTime.Now.AddDays(-15)
                        //    },
                        //    new MemberNote
                        //    {
                        //        Id = 4,
                        //        MemberId = 4,
                        //        Note = "Final meeting to present the completed CRM project and gather client feedback.",
                        //        CreatedAt = DateTime.Now.AddDays(-5)
                        //    },
                        //    new MemberNote
                        //    {
                        //        Id = 5,
                        //        MemberId = 5,
                        //        Note = "Discussed potential upgrades to the CRM system and next steps for implementation.",
                        //        CreatedAt = DateTime.Now.AddDays(-1)
                        //    }
                        //);


                        //new ContactNote
                        //{
                        //    Id = 1,
                        //    ContactId = 1,
                        //    Note = "Followed up with the client regarding additional customization needs for the CRM system.",
                        //    CreatedAt = DateTime.Now.AddDays(-25)
                        //},
                    //    new ContactNote
                    //    {
                    //        Id = 2,
                    //        ContactId = 2,
                    //        Note = "Provided the client with a detailed overview of CRM features during the demonstration.",
                    //        CreatedAt = DateTime.Now.AddDays(-18)
                    //    },
                    //    new ContactNote
                    //    {
                    //        Id = 3,
                    //        ContactId = 3,
                    //        Note = "Client requested a new feature to track employee performance metrics.",
                    //        CreatedAt = DateTime.Now.AddDays(-12)
                    //    },
                    //    new ContactNote
                    //    {
                    //        Id = 4,
                    //        ContactId = 4,
                    //        Note = "Inquired about potential integrations with the existing HR software.",
                    //        CreatedAt = DateTime.Now.AddDays(-7)
                    //    },
                    //    new ContactNote
                    //    {
                    //        Id = 5,
                    //        ContactId = 5,
                    //        Note = "Arranged a meeting to discuss possible CRM upgrade options and pricing.",
                    //        CreatedAt = DateTime.Now.AddDays(-2)
                    //    }
                    //);
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
