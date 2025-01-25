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
                                TypeName = "Basic Membership",
                                TypeDescr = "Access to gym equipment and locker room facilities."
                            },
                            new MembershipType
                            {
                                ID = 2,
                                TypeName = "Premium Membership",
                                TypeDescr = "Includes Basic Membership benefits plus access to group classes and pool."
                            },
                            new MembershipType
                            {
                                ID = 3,
                                TypeName = "Family Membership",
                                TypeDescr = "Includes Premium Membership benefits for up to 4 family members."
                            },
                            new MembershipType
                            {
                                ID = 4,
                                TypeName = "Student Membership",
                                TypeDescr = "Discounted membership for students with valId Id."
                            },
                            new MembershipType
                            {
                                ID = 5,
                                TypeName = "Corporate Membership",
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
                            //MemberLastName = "Adam",
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
                            //MemberLastName = "Lamay",
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
                            //MemberLastName = "Con",
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
                            MemberLastName = "Rody",
                            //MemberLastName = "Harris",
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

                        }
                        //new Member
                        //{
                        //    Id = 11,
                        //    MemberFirstName = "Liam",
                        //    
                        //    MemberLastName = "Garcia",
                        //    JoinDate = new DateTime(2021, 7, 10),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 11,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "321 Birch Parkway",
                        //        AddressLineTwo = null,
                        //        City = "Forgestone",
                        //        StateProvince = "Metal Hills",
                        //        PostalCode = "B3P7K9",
                        //        Country = "USA"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 12,
                        //    MemberFirstName = "Sophia",
                        //    MemberLastName = "Eeren",
                        //    MemberLastName = "Rodriguez",
                        //    JoinDate = new DateTime(2023, 3, 15),
                        //    StandingStatus = StandingStatus.Cancelled,
                        //    OrganizationId = 12,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "789 Poplar Court",
                        //        AddressLineTwo = "Apt 12C",
                        //        City = "Alloyton",
                        //        StateProvince = "Forge Land",
                        //        PostalCode = "P1L3A8",
                        //        Country = "UK"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 13,
                        //    MemberFirstName = "Benjamin",
                        //    
                        //    MemberLastName = "Clark",
                        //    JoinDate = new DateTime(2020, 8, 19),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 13,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "102 Maple Circle",
                        //        AddressLineTwo = null,
                        //        City = "Copperlake",
                        //        StateProvince = "Metal Coast",
                        //        PostalCode = "M2C4L7",
                        //        Country = "USA"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 14,
                        //    MemberFirstName = "Emma",
                        //    
                        //    MemberLastName = "Lopez",
                        //    JoinDate = new DateTime(2021, 5, 20),
                        //    StandingStatus = StandingStatus.Inactive,
                        //    OrganizationId = 14,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "455 Oak Crescent",
                        //        AddressLineTwo = "Suite 8",
                        //        City = "Iron Bay",
                        //        StateProvince = "Steel Region",
                        //        PostalCode = "O5C8L4",
                        //        Country = "Canada"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 15,
                        //    MemberFirstName = "Alexander",
                        //    
                        //    MemberLastName = "Walker",
                        //    JoinDate = new DateTime(2022, 11, 18),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 15,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "678 Elmwood Drive",
                        //        AddressLineTwo = null,
                        //        City = "Iron Harbor",
                        //        StateProvince = "Metal Territory",
                        //        PostalCode = "E5L8M2",
                        //        Country = "USA"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 16,
                        //    MemberFirstName = "Isabella",
                        //    MemberLastName = "Maria",
                        //    MemberLastName = "Young",
                        //    JoinDate = new DateTime(2023, 4, 5),
                        //    StandingStatus = StandingStatus.Cancelled,
                        //    OrganizationId = 16,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "150 Pine View Lane",
                        //        AddressLineTwo = "Unit 11A",
                        //        City = "Steel RIdge",
                        //        StateProvince = "Forge County",
                        //        PostalCode = "P2V3L1",
                        //        Country = "Canada"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 17,
                        //    MemberFirstName = "Ethan",
                        //    MemberLastName = "Jordan",
                        //    MemberLastName = "Hall",
                        //    JoinDate = new DateTime(2019, 9, 30),
                        //    StandingStatus = StandingStatus.Inactive,
                        //    OrganizationId = 17,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "982 Cedar Grove",
                        //        AddressLineTwo = null,
                        //        City = "Alloyville",
                        //        StateProvince = "Iron District",
                        //        PostalCode = "C9D8V6",
                        //        Country = "USA"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 18,
                        //    MemberFirstName = "Mia",
                        //    
                        //    MemberLastName = "Adams",
                        //    JoinDate = new DateTime(2020, 7, 15),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 18,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "455 Redwood Trail",
                        //        AddressLineTwo = "Building 2",
                        //        City = "Forge City",
                        //        StateProvince = "Steel Plateau",
                        //        PostalCode = "R1D6C8",
                        //        Country = "UK"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 19,
                        //    MemberFirstName = "Daniel",
                        //    MemberLastName = "Amber",
                        //    MemberLastName = "Hernandez",
                        //    JoinDate = new DateTime(2021, 10, 10),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 19,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "210 Birchwood Terrace",
                        //        AddressLineTwo = null,
                        //        City = "Copperton",
                        //        StateProvince = "Forge Hills",
                        //        PostalCode = "B7L9P3",
                        //        Country = "Australia"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 20,
                        //    MemberFirstName = "Ava",
                        //    
                        //    MemberLastName = "Roberts",
                        //    JoinDate = new DateTime(2022, 12, 25),
                        //    StandingStatus = StandingStatus.Inactive,
                        //    OrganizationId = 20,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "300 Maple Valley",
                        //        AddressLineTwo = "Suite 7B",
                        //        City = "Iron BrIdge",
                        //        StateProvince = "Metal County",
                        //        PostalCode = "M8C9B5",
                        //        Country = "USA"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 21,
                        //    MemberFirstName = "Lucas",
                        //    MemberLastName = "Rody",
                        //    MemberLastName = "Perez",
                        //    JoinDate = new DateTime(2018, 3, 3),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 21,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "150 Granite Street",
                        //        AddressLineTwo = "Floor 1",
                        //        City = "Steel Heights",
                        //        StateProvince = "Iron Plains",
                        //        PostalCode = "G1N5T8",
                        //        Country = "UK"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 22,
                        //    MemberFirstName = "Charlotte",
                        //    MemberLastName = "L.",
                        //    MemberLastName = "King",
                        //    JoinDate = new DateTime(2023, 8, 20),
                        //    StandingStatus = StandingStatus.Cancelled,
                        //    OrganizationId = 22,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "489 Walnut Road",
                        //        AddressLineTwo = null,
                        //        City = "Forgeland",
                        //        StateProvince = "Metal Shores",
                        //        PostalCode = "W5R9K2",
                        //        Country = "Canada"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 23,
                        //    MemberFirstName = "Henry",
                        //    
                        //    MemberLastName = "Scott",
                        //    JoinDate = new DateTime(2020, 11, 8),
                        //    StandingStatus = StandingStatus.Inactive,
                        //    OrganizationId = 23,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "670 Cedar Hills",
                        //        AddressLineTwo = "Building 5",
                        //        City = "Ironwood",
                        //        StateProvince = "Steel Peninsula",
                        //        PostalCode = "I2R7L3",
                        //        Country = "USA"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 24,
                        //    MemberFirstName = "Amelia",
                        //    MemberLastName = "New",
                        //    MemberLastName = "Green",
                        //    JoinDate = new DateTime(2021, 2, 2),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 24,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "90 Ashwood Lane",
                        //        AddressLineTwo = "Unit 6A",
                        //        City = "Copper RIdge",
                        //        StateProvince = "Metal Valley",
                        //        PostalCode = "A1L3T7",
                        //        Country = "Australia"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 25,
                        //    MemberFirstName = "Jack",
                        //    
                        //    MemberLastName = "White",
                        //    JoinDate = new DateTime(2019, 12, 15),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 25,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "300 Maple Hill",
                        //        AddressLineTwo = null,
                        //        City = "Steelport",
                        //        StateProvince = "Iron Range",
                        //        PostalCode = "M4R2N1",
                        //        Country = "USA"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 26,
                        //    MemberFirstName = "Olivia",
                        //    MemberLastName = "Eron",
                        //    MemberLastName = "Taylor",
                        //    JoinDate = new DateTime(2020, 4, 18),
                        //    StandingStatus = StandingStatus.Inactive,
                        //    OrganizationId = 26,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "500 Birchwood Avenue",
                        //        AddressLineTwo = null,
                        //        City = "Forgedale",
                        //        StateProvince = "Metal Coast",
                        //        PostalCode = "F6G8H4",
                        //        Country = "UK"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 27,
                        //    MemberFirstName = "Noah",
                        //    MemberLastName = "Hood",
                        //    MemberLastName = "Martinez",
                        //    JoinDate = new DateTime(2023, 7, 25),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 27,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "789 Ironwood Lane",
                        //        AddressLineTwo = "Suite 3B",
                        //        City = "Steelton",
                        //        StateProvince = "Forge Province",
                        //        PostalCode = "S3T5R2",
                        //        Country = "Canada"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 28,
                        //    MemberFirstName = "Sophia",
                        //    
                        //    MemberLastName = "Garcia",
                        //    JoinDate = new DateTime(2021, 5, 12),
                        //    StandingStatus = StandingStatus.Cancelled,
                        //    OrganizationId = 28,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "213 Maplewood Drive",
                        //        AddressLineTwo = "Apt 14",
                        //        City = "Ironville",
                        //        StateProvince = "Steel Heights",
                        //        PostalCode = "I5R6T8",
                        //        Country = "USA"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 29,
                        //    MemberFirstName = "Mason",
                        //    MemberLastName = "Kotlyn",
                        //    MemberLastName = "Thomas",
                        //    JoinDate = new DateTime(2018, 9, 1),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 29,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "112 Copper Valley Road",
                        //        AddressLineTwo = null,
                        //        City = "Alloytown",
                        //        StateProvince = "Metal District",
                        //        PostalCode = "C8D2R4",
                        //        Country = "Australia"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 30,
                        //    MemberFirstName = "Ella",
                        //    
                        //    MemberLastName = "Harris",
                        //    JoinDate = new DateTime(2022, 3, 17),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 30,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "75 Elmwood Circle",
                        //        AddressLineTwo = "Floor 2",
                        //        City = "Forgeland",
                        //        StateProvince = "Iron Region",
                        //        PostalCode = "E7F8G9",
                        //        Country = "USA"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 31,
                        //    MemberFirstName = "James",
                        //    MemberLastName = "Lopez",
                        //    MemberLastName = "Nelson",
                        //    JoinDate = new DateTime(2020, 12, 22),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 31,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "680 Pine Crest Road",
                        //        AddressLineTwo = null,
                        //        City = "Steel RIdge",
                        //        StateProvince = "Metal Canyon",
                        //        PostalCode = "P6R9T3",
                        //        Country = "Canada"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 32,
                        //    MemberFirstName = "Grace",
                        //    MemberLastName = "Martinez",
                        //    MemberLastName = "Clark",
                        //    JoinDate = new DateTime(2019, 7, 10),
                        //    StandingStatus = StandingStatus.Inactive,
                        //    OrganizationId = 32,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "145 Ashwood Lane",
                        //        AddressLineTwo = "Unit 9",
                        //        City = "Iron Harbor",
                        //        StateProvince = "Forge Peninsula",
                        //        PostalCode = "A2L5C8",
                        //        Country = "UK"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 33,
                        //    MemberFirstName = "Benjamin",
                        //    
                        //    MemberLastName = "Lewis",
                        //    JoinDate = new DateTime(2021, 10, 14),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 33,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "390 Cedar Park Drive",
                        //        AddressLineTwo = null,
                        //        City = "Copperton",
                        //        StateProvince = "Metal Zone",
                        //        PostalCode = "C3T6R2",
                        //        Country = "USA"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 34,
                        //    MemberFirstName = "Emily",
                        //    MemberLastName = "Nethan",
                        //    MemberLastName = "Young",
                        //    JoinDate = new DateTime(2022, 5, 27),
                        //    StandingStatus = StandingStatus.Cancelled,
                        //    OrganizationId = 34,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "870 Birch View Lane",
                        //        AddressLineTwo = "Suite 2A",
                        //        City = "Steelville",
                        //        StateProvince = "Iron Plateau",
                        //        PostalCode = "B8L2M5",
                        //        Country = "Australia"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 35,
                        //    MemberFirstName = "Henry",
                        //    MemberLastName = "A.",
                        //    MemberLastName = "Walker",
                        //    JoinDate = new DateTime(2019, 4, 30),
                        //    StandingStatus = StandingStatus.Inactive,
                        //    OrganizationId = 35,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "540 Maple Street",
                        //        AddressLineTwo = null,
                        //        City = "Alloyport",
                        //        StateProvince = "Forge Territory",
                        //        PostalCode = "M5N3T9",
                        //        Country = "USA"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 36,
                        //    MemberFirstName = "Abigail",
                        //    MemberLastName = "Ember",
                        //    MemberLastName = "Hill",
                        //    JoinDate = new DateTime(2020, 6, 18),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 36,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "235 Ironwood Blvd",
                        //        AddressLineTwo = "Unit 10",
                        //        City = "Forgeland",
                        //        StateProvince = "Metal Highlands",
                        //        PostalCode = "F4G8T2",
                        //        Country = "Canada"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 37,
                        //    MemberFirstName = "Lucas",
                        //    MemberLastName = "Ralph",
                        //    MemberLastName = "Carter",
                        //    JoinDate = new DateTime(2021, 1, 19),
                        //    StandingStatus = StandingStatus.Cancelled,
                        //    OrganizationId = 37,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "999 Oak Valley",
                        //        AddressLineTwo = "Building 8",
                        //        City = "Iron Heights",
                        //        StateProvince = "Steel Peninsula",
                        //        PostalCode = "I7R4M3",
                        //        Country = "USA"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 38,
                        //    MemberFirstName = "Chloe",
                        //    MemberLastName = "Lindsy",
                        //    MemberLastName = "Green",
                        //    JoinDate = new DateTime(2022, 9, 12),
                        //    StandingStatus = StandingStatus.Inactive,
                        //    OrganizationId = 38,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "455 Maple View",
                        //        AddressLineTwo = null,
                        //        City = "Steel RIdge",
                        //        StateProvince = "Forge Zone",
                        //        PostalCode = "M6R2L8",
                        //        Country = "UK"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 39,
                        //    MemberFirstName = "Mason",
                        //    
                        //    MemberLastName = "Evans",
                        //    JoinDate = new DateTime(2018, 8, 10),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 39,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "67 Elm Park Road",
                        //        AddressLineTwo = "Suite 1C",
                        //        City = "Iron Valley",
                        //        StateProvince = "Metal Canyon",
                        //        PostalCode = "E2R5L9",
                        //        Country = "Australia"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 40,
                        //    MemberFirstName = "Amelia",
                        //    MemberLastName = "Kaper",
                        //    MemberLastName = "Perez",
                        //    JoinDate = new DateTime(2019, 3, 5),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 40,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "310 Birch Grove",
                        //        AddressLineTwo = null,
                        //        City = "Alloytown",
                        //        StateProvince = "Steel Coast",
                        //        PostalCode = "A3N5M1",
                        //        Country = "USA"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 41,
                        //    MemberFirstName = "James",
                        //    
                        //    MemberLastName = "Reed",
                        //    JoinDate = new DateTime(2020, 11, 25),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 41,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "175 Pinewood Ave",
                        //        AddressLineTwo = "Floor 3",
                        //        City = "Copperport",
                        //        StateProvince = "Iron RIdge",
                        //        PostalCode = "P9R3L7",
                        //        Country = "Canada"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 42,
                        //    MemberFirstName = "Olivia",
                        //    MemberLastName = "Marie",
                        //    MemberLastName = "Harris",
                        //    JoinDate = new DateTime(2022, 7, 14),
                        //    StandingStatus = StandingStatus.Cancelled,
                        //    OrganizationId = 42,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "890 Maple Heights",
                        //        AddressLineTwo = "Unit 12",
                        //        City = "Steelton",
                        //        StateProvince = "Forge Region",
                        //        PostalCode = "S4T8M6",
                        //        Country = "USA"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 43,
                        //    MemberFirstName = "Liam",
                        //    MemberLastName = "Toot",
                        //    MemberLastName = "Campbell",
                        //    JoinDate = new DateTime(2021, 6, 21),
                        //    StandingStatus = StandingStatus.Inactive,
                        //    OrganizationId = 43,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "650 Ironwood Lane",
                        //        AddressLineTwo = "Suite 9B",
                        //        City = "Forgeland",
                        //        StateProvince = "Metal Territory",
                        //        PostalCode = "F8L2T3",
                        //        Country = "UK"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 44,
                        //    MemberFirstName = "Sophia",
                        //    MemberLastName = "Root",
                        //    MemberLastName = "King",
                        //    JoinDate = new DateTime(2019, 5, 30),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 44,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "100 Elmview Road",
                        //        AddressLineTwo = "Apt 3D",
                        //        City = "Steel RIdge",
                        //        StateProvince = "Iron Zone",
                        //        PostalCode = "E9R4M2",
                        //        Country = "Australia"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 45,
                        //    MemberFirstName = "Emma",
                        //    MemberLastName = "N.",
                        //    MemberLastName = "Lewis",
                        //    JoinDate = new DateTime(2023, 2, 17),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 45,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "360 Maple Grove",
                        //        AddressLineTwo = null,
                        //        City = "Alloytown",
                        //        StateProvince = "Metal Shores",
                        //        PostalCode = "M2R5L8",
                        //        Country = "Canada"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 46,
                        //    MemberFirstName = "Benjamin",
                        //    
                        //    MemberLastName = "Moore",
                        //    JoinDate = new DateTime(2021, 9, 9),
                        //    StandingStatus = StandingStatus.Cancelled,
                        //    OrganizationId = 1,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "550 Cedar Valley",
                        //        AddressLineTwo = "Suite 4A",
                        //        City = "Copperport",
                        //        StateProvince = "Iron Valley",
                        //        PostalCode = "C6N3M4",
                        //        Country = "USA"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 47,
                        //    MemberFirstName = "Isabella",
                        //    MemberLastName = "F.",
                        //    MemberLastName = "Morgan",
                        //    JoinDate = new DateTime(2020, 4, 11),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 2,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "770 Oakleaf Drive",
                        //        AddressLineTwo = null,
                        //        City = "Metal City",
                        //        StateProvince = "Iron Plains",
                        //        PostalCode = "O1M3T7",
                        //        Country = "Canada"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 48,
                        //    MemberFirstName = "Alexander",
                        //    MemberLastName = "Jeremie",
                        //    MemberLastName = "Taylor",
                        //    JoinDate = new DateTime(2022, 10, 7),
                        //    StandingStatus = StandingStatus.Inactive,
                        //    OrganizationId = 3,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "145 Steel Valley",
                        //        AddressLineTwo = "Building 5C",
                        //        City = "Forgeland",
                        //        StateProvince = "Metal Heights",
                        //        PostalCode = "S3V5L2",
                        //        Country = "UK"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 49,
                        //    MemberFirstName = "Mia",
                        //    MemberLastName = "Han",
                        //    MemberLastName = "Baker",
                        //    JoinDate = new DateTime(2021, 3, 3),
                        //    StandingStatus = StandingStatus.Good,
                        //    OrganizationId = 4,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "92 Maple Way",
                        //        AddressLineTwo = null,
                        //        City = "Steelton",
                        //        StateProvince = "Forge Valley",
                        //        PostalCode = "M7T2L9",
                        //        Country = "Australia"
                        //    }
                        //},
                        //new Member
                        //{
                        //    Id = 50,
                        //    MemberFirstName = "Elijah",
                        //    
                        //    MemberLastName = "Hughes",
                        //    JoinDate = new DateTime(2019, 12, 15),
                        //    StandingStatus = StandingStatus.Cancelled,
                        //    OrganizationId = 5,
                        //    Address = new Address
                        //    {
                        //        AddressLineOne = "333 Cedar Trail",
                        //        AddressLineTwo = "Apt 8F",
                        //        City = "Iron Heights",
                        //        StateProvince = "Metal District",
                        //        PostalCode = "C4N8M5",
                        //        Country = "USA"
                        //    }
                        //}
                        );
                        context.SaveChanges();
                    }
                    if (!context.MemberIndustries.Any())
                    {
                        context.MemberIndustries.AddRange(
                            new MemberIndustry { MemberId = 1, IndustryId = 1 },
    new MemberIndustry { MemberId = 2, IndustryId = 2 },
    new MemberIndustry { MemberId = 3, IndustryId = 3 },
    new MemberIndustry { MemberId = 4, IndustryId = 4 },
    new MemberIndustry { MemberId = 5, IndustryId = 5 },
    new MemberIndustry { MemberId = 6, IndustryId = 6 },
    new MemberIndustry { MemberId = 7, IndustryId = 7 },
    new MemberIndustry { MemberId = 8, IndustryId = 8 },
    new MemberIndustry { MemberId = 9, IndustryId = 9 },
    new MemberIndustry { MemberId = 10, IndustryId = 10 });
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
                            }
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 11, // Mason Lee
                        //        MembershipTypeId = 5 //
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 12,
                        //        MembershipTypeId = 4 // Student Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 13,
                        //        MembershipTypeId = 2 // Premium Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 14,
                        //        MembershipTypeId = 3 // Family Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 15,
                        //        MembershipTypeId = 5 // Corporate Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 16,
                        //        MembershipTypeId = 1 // Basic Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 17,
                        //        MembershipTypeId = 4 // Student Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 18,
                        //        MembershipTypeId = 2 // Premium Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 19,
                        //        MembershipTypeId = 3 // Family Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 20,
                        //        MembershipTypeId = 5 // Corporate Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 21,
                        //        MembershipTypeId = 1 // Basic Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 22,
                        //        MembershipTypeId = 4 // Student Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 23,
                        //        MembershipTypeId = 2 // Premium Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 24,
                        //        MembershipTypeId = 3 // Family Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 25,
                        //        MembershipTypeId = 5 // Corporate Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 26,
                        //        MembershipTypeId = 1 // Basic Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 27,
                        //        MembershipTypeId = 4 // Student Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 28,
                        //        MembershipTypeId = 2 // Premium Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 29,
                        //        MembershipTypeId = 3 // Family Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 30,
                        //        MembershipTypeId = 5 // Corporate Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 31,
                        //        MembershipTypeId = 1 // Basic Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 32,
                        //        MembershipTypeId = 4 // Student Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 33,
                        //        MembershipTypeId = 2 // Premium Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 34,
                        //        MembershipTypeId = 3 // Family Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 35,
                        //        MembershipTypeId = 5 // Corporate Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 36,
                        //        MembershipTypeId = 1 // Basic Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 37,
                        //        MembershipTypeId = 4 // Student Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 38,
                        //        MembershipTypeId = 2 // Premium Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 39,
                        //        MembershipTypeId = 3 // Family Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 40,
                        //        MembershipTypeId = 5 // Corporate Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 41,
                        //        MembershipTypeId = 1 // Basic Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 42,
                        //        MembershipTypeId = 4 // Student Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 43,
                        //        MembershipTypeId = 2 // Premium Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 44,
                        //        MembershipTypeId = 3 // Family Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 45,
                        //        MembershipTypeId = 5 // Corporate Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 46,
                        //        MembershipTypeId = 1 // Basic Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 47,
                        //        MembershipTypeId = 4 // Student Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 48,
                        //        MembershipTypeId = 2 // Premium Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 49,
                        //        MembershipTypeId = 3 // Family Membership
                        //    },
                        //    new MemberMembershipType
                        //    {
                        //        MemberId = 50,
                        //        MembershipTypeId = 5 // Corporate Membership
                        //    }
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
                    //            OrganizationId = 1,  // Assuming Organization with Id 1 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 1, 5),
                    //            //    InteractionNote = "Initial discussion on potential partnership."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Software Development for HealthCorp",
                    //            OpportunityDescr = "Software development project for HealthCorp to enhance their internal systems.",
                    //            OpportunityStatus = OpportunityStatus.InProgress,
                    //            OrganizationId = 2,  // Assuming Organization with Id 2 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 1, 10),
                    //            //    InteractionNote = "Meeting to finalize project requirements."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Website Overhaul for FinServe",
                    //            OpportunityDescr = "Website redesign project for FinServe to improve their online presence.",
                    //            OpportunityStatus = OpportunityStatus.Closed,
                    //            OrganizationId = 3,  // Assuming Organization with Id 3 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 1, 15),
                    //            //    InteractionNote = "Final meeting to close project details."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Mobile App Development for EduTech",
                    //            OpportunityDescr = "Development of a mobile app for EduTech to expand their reach in the education sector.",
                    //            OpportunityStatus = OpportunityStatus.Open,
                    //            OrganizationId = 4,  // Assuming Organization with Id 4 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 1, 18),
                    //            //    InteractionNote = "Kickoff meeting for mobile app project."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "CRM System for SalesForce",
                    //            OpportunityDescr = "Implementation of a CRM system for SalesForce to improve their customer relationship management.",
                    //            OpportunityStatus = OpportunityStatus.InProgress,
                    //            OrganizationId = 5,  // Assuming Organization with Id 5 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 1, 20),
                    //            //    InteractionNote = "Meeting to discuss CRM system features."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "E-commerce Platform for ShopMart",
                    //            OpportunityDescr = "Development of a full-fledged e-commerce platform for ShopMart.",
                    //            OpportunityStatus = OpportunityStatus.Closed,
                    //            OrganizationId = 6,  // Assuming Organization with Id 6 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 1, 22),
                    //            //    InteractionNote = "Final review meeting before project closure."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "AI Integration for SmartTech",
                    //            OpportunityDescr = "Integration of AI-based solutions for SmartTech's systems.",
                    //            OpportunityStatus = OpportunityStatus.Open,
                    //            OrganizationId = 7,  // Assuming Organization with Id 7 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 1, 25),
                    //            //    InteractionNote = "Discussion about AI integration and scope."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Cloud Migration for DataCloud",
                    //            OpportunityDescr = "Cloud migration for DataCloud to streamline their operations and storage.",
                    //            OpportunityStatus = OpportunityStatus.InProgress,
                    //            OrganizationId = 8,  // Assuming Organization with Id 8 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 1, 28),
                    //            //    InteractionNote = "Discussing cloud architecture for migration."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Cybersecurity for SecureNet",
                    //            OpportunityDescr = "Cybersecurity services for SecureNet to enhance their data protection.",
                    //            OpportunityStatus = OpportunityStatus.Closed,
                    //            OrganizationId = 9,  // Assuming Organization with Id 9 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 1, 30),
                    //            //    InteractionNote = "Final agreement on cybersecurity solutions."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Data Analytics for AnalyticsPro",
                    //            OpportunityDescr = "Implementing a data analytics platform for AnalyticsPro to improve decision-making.",
                    //            OpportunityStatus = OpportunityStatus.Open,
                    //            OrganizationId = 10,  // Assuming Organization with Id 10 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 2, 2),
                    //            //    InteractionNote = "Initial discussion on data analytics requirements."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Business Intelligence for BizIntel",
                    //            OpportunityDescr = "ProvIding business intelligence solutions for BizIntel to enhance reporting capabilities.",
                    //            OpportunityStatus = OpportunityStatus.InProgress,
                    //            OrganizationId = 11,  // Assuming Organization with Id 11 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 2, 5),
                    //            //    InteractionNote = "Meeting to review BI system features."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "ERP System for GlobalCorp",
                    //            OpportunityDescr = "ERP system implementation for GlobalCorp to streamline operations.",
                    //            OpportunityStatus = OpportunityStatus.Closed,
                    //            OrganizationId = 12,  // Assuming Organization with Id 12 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 2, 7),
                    //            //    InteractionNote = "Reviewing implementation plan and timeline."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Virtual Events Platform for EventPro",
                    //            OpportunityDescr = "Development of a platform for virtual events for EventPro.",
                    //            OpportunityStatus = OpportunityStatus.Open,
                    //            OrganizationId = 13,  // Assuming Organization with Id 13 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 2, 10),
                    //            //    InteractionNote = "Kickoff meeting to discuss platform features."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Logistics Management System for MoveIt",
                    //            OpportunityDescr = "Logistics management software for MoveIt to optimize their operations.",
                    //            OpportunityStatus = OpportunityStatus.InProgress,
                    //            OrganizationId = 14,  // Assuming Organization with Id 14 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 2, 12),
                    //            //    InteractionNote = "Discussing project scope and logistics software needs."
                    //            //}
                    //        },
                    //        new Opportunity
                    //        {
                    //            OpportunityName = "Blockchain Solutions for ChainTech",
                    //            OpportunityDescr = "Developing blockchain-based solutions for ChainTech's supply chain management.",
                    //            OpportunityStatus = OpportunityStatus.Closed,
                    //            OrganizationId = 15,  // Assuming Organization with Id 15 exists
                    //            //Interaction = new Interaction
                    //            //{
                    //            //    InteractionDate = new DateTime(2025, 2, 14),
                    //            //    InteractionNote = "Final meeting on blockchain integration."
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
                    //            InteractionNote = "Initial contact for potential collaboration.",
                    //            ContactId = 1,
                    //            MemberID = 1,
                    //            OpportunityId = 1
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 3),
                    //            InteractionNote = "Follow-up Email regarding partnership details.",
                    //            ContactId = 2,
                    //            MemberID = 2,
                    //            OpportunityId = 2
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 5),
                    //            InteractionNote = "Scheduled call to discuss project needs.",
                    //            ContactId = 3,
                    //            MemberID = 3,
                    //            OpportunityId = 3
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 10),
                    //            InteractionNote = "Met to discuss contract terms and conditions.",
                    //            ContactId = 4,
                    //            MemberID = 4,
                    //            OpportunityId = 4
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 12),
                    //            InteractionNote = "Client inquiry on pricing models.",
                    //            ContactId = 5,
                    //            MemberID = 1,
                    //            OpportunityId = 5
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 15),
                    //            InteractionNote = "Discussed solution packages for enterprise clients.",
                    //            ContactId = 6,
                    //            MemberID = 2,
                    //            OpportunityId = 6
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 18),
                    //            InteractionNote = "Following up on service proposal.",
                    //            ContactId = 7,
                    //            MemberID = 3,
                    //            OpportunityId = 7
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 20),
                    //            InteractionNote = "Finalizing service agreement terms.",
                    //            ContactId = 8,
                    //            MemberID = 4,
                    //            OpportunityId = 8
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 22),
                    //            InteractionNote = "Agreement on next steps and deliverables.",
                    //            ContactId = 9,
                    //            MemberID = 1,
                    //            OpportunityId = 9
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 25),
                    //            InteractionNote = "Reviewing deliverables for upcoming project.",
                    //            ContactId = 10,
                    //            MemberID = 2,
                    //            OpportunityId = 10
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 28),
                    //            InteractionNote = "Update on progress and timeline.",
                    //            ContactId = 11,
                    //            MemberID = 3,
                    //            OpportunityId = 11
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 1, 30),
                    //            InteractionNote = "Follow-up on final proposal details.",
                    //            ContactId = 12,
                    //            MemberID = 4,
                    //            OpportunityId = 12
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 2, 2),
                    //            InteractionNote = "Final meeting before project launch.",
                    //            ContactId = 13,
                    //            MemberID = 1,
                    //            OpportunityId = 13
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 2, 5),
                    //            InteractionNote = "Confirming details of service agreement.",
                    //            ContactId = 14,
                    //            MemberID = 2,
                    //            OpportunityId = 14
                    //        },
                    //        new Interaction
                    //        {
                    //            InteractionDate = new DateTime(2025, 2, 7),
                    //            InteractionNote = "Meeting to finalize documentation.",
                    //            ContactId = 3,
                    //            MemberID = 3,
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
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 3
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 12),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 4
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 18),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 5
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 20),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 6
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 22),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 7
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 25),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 8
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 27),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 9
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 30),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 10
                            }
                            //new Cancellation
                            //{
                            //    CancellationDate = new DateTime(2025, 2, 1),
                            //    Canceled = false,
                            //    CancellationNote = "Active member, no cancellation.",
                            //    MemberID = 11
                            //},
                            //new Cancellation
                            //{
                            //    CancellationDate = new DateTime(2025, 2, 5),
                            //    Canceled = false,
                            //    CancellationNote = "Active member, no cancellation.",
                            //    MemberID = 12
                            //},
                            //new Cancellation
                            //{
                            //    CancellationDate = new DateTime(2025, 2, 10),
                            //    Canceled = false,
                            //    CancellationNote = "Active member, no cancellation.",
                            //    MemberID = 13
                            //},
                            //new Cancellation
                            //{
                            //    CancellationDate = new DateTime(2025, 2, 15),
                            //    Canceled = false,
                            //    CancellationNote = "Active member, no cancellation.",
                            //    MemberID = 14
                            //},
                            //new Cancellation
                            //{
                            //    CancellationDate = new DateTime(2025, 2, 18),
                            //    Canceled = false,
                            //    CancellationNote = "Active member, no cancellation.",
                            //    MemberID = 15
                            //}
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
                                     //IsActive = true
                                 },
                                 new ProductionEmail
                                 {
                                     Id = 2,
                                     EmailType = "Renewal Reminder",
                                     Subject = "Membership Renewal Reminder",
                                     Body = $"Dear {randomNames[1]},\n\nYour membership with NIA is about to expire on 2025-02-15. Please renew your membership to continue enjoying all the benefits.\n\nBest regards,\nNIA Team",
                                     //IsActive = true
                                 },
                                 new ProductionEmail
                                 {
                                     Id = 3,
                                     EmailType = "Cancellation Notice",
                                     Subject = "Membership Cancellation Confirmation",
                                     Body = $"Dear {randomNames[2]},\n\nWe are sorry to see you go. Your membership has been successfully canceled. If you change your mind in the future, we’d love to have you back.\n\nBest regards,\nNIA Team",
                                     //IsActive = true
                                 },
                                 new ProductionEmail
                                 {
                                     Id = 4,
                                     EmailType = "Membership Update",
                                     Subject = "Important Membership Update",
                                     Body = $"Dear {randomNames[3]},\n\nWe would like to inform you of an important update regarding your membership status. Please log in to your account for more details.\n\nBest regards,\nNIA Team",
                                     //IsActive = true
                                 },
                                 new ProductionEmail
                                 {
                                     Id = 5,
                                     EmailType = "General Notification",
                                     Subject = "NIA System Update",
                                     Body = $"Dear {randomNames[4]},\n\nThis is a notification regarding a recent update to the NIA system. We encourage you to check out the new features and improvements.\n\nBest regards,\nNIA Team",
                                     //IsActive = true
                                 });
                            context.SaveChanges();
                        }
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
