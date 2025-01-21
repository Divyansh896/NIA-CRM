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
                }

                try
                {
                    //Add some Class Start times
                    if (!context.Industries.Any())
                    {
                        context.Industries.AddRange(
                            new Industry { ID = 1, IndustryName = "Alpha Steel" },
                            new Industry { ID = 2, IndustryName = "TISCO CO." },
                            new Industry { ID = 3, IndustryName = "M Time Irons" },
                            new Industry { ID = 4, IndustryName = "Forge & Foundry Inc." },
                            new Industry { ID = 5, IndustryName = "Northern Metalworks" },
                            new Industry { ID = 6, IndustryName = "Titanium Solutions" },
                            new Industry { ID = 7, IndustryName = "Phoenix Alloys" },
                            new Industry { ID = 8, IndustryName = "Galaxy Metals" },
                            new Industry { ID = 9, IndustryName = "Ironclad Industries" },
                            new Industry { ID = 10, IndustryName = "Silverline Fabrication" },
                            new Industry { ID = 11, IndustryName = "Star Steelworks" },
                            new Industry { ID = 12, IndustryName = "Summit Metal Co." },
                            new Industry { ID = 13, IndustryName = "Everest Iron Corp." },
                            new Industry { ID = 14, IndustryName = "Prime Alloy Coatings" },
                            new Industry { ID = 15, IndustryName = "Magnum Steel Solutions" }
                        );
                        context.SaveChanges();
                    }
                    if (!context.Organizations.Any())
                    {
                        context.Organizations.AddRange(
                            new Organization
                            {
                                ID = 1,
                                OrganizationName = "Steel Works Ltd.",
                                OrganizationSize = 150,
                                OrganizationWeb = "https://www.steelworksltd.com",
                                IndustryID = 1 // Related to "Alpha Steel"
                            },
                            new Organization
                            {
                                ID = 2,
                                OrganizationName = "TISCO Technologies",
                                OrganizationSize = 200,
                                OrganizationWeb = "https://www.tiscotech.com",
                                IndustryID = 2 // Related to "TISCO CO."
                            },
                            new Organization
                            {
                                ID = 3,
                                OrganizationName = "Iron Builders Inc.",
                                OrganizationSize = 300,
                                OrganizationWeb = "https://www.ironbuilders.com",
                                IndustryID = 3 // Related to "M Time Irons"
                            },
                            new Organization
                            {
                                ID = 4,
                                OrganizationName = "Forge Foundry Co.",
                                OrganizationSize = 100,
                                OrganizationWeb = "https://www.forgefoundry.com",
                                IndustryID = 4 // Related to "Forge & Foundry Inc."
                            },
                            new Organization
                            {
                                ID = 5,
                                OrganizationName = "Northern Metal Solutions",
                                OrganizationSize = 180,
                                OrganizationWeb = "https://www.northernmetals.com",
                                IndustryID = 5 // Related to "Northern Metalworks"
                            },
                            new Organization
                            {
                                ID = 6,
                                OrganizationName = "Titanium Group Ltd.",
                                OrganizationSize = 250,
                                OrganizationWeb = "https://www.titaniumgroup.com",
                                IndustryID = 6 // Related to "Titanium Solutions"
                            },
                            new Organization
                            {
                                ID = 7,
                                OrganizationName = "Phoenix Alloy Works",
                                OrganizationSize = 320,
                                OrganizationWeb = "https://www.phoenixalloy.com",
                                IndustryID = 7 // Related to "Phoenix Alloys"
                            },
                            new Organization
                            {
                                ID = 8,
                                OrganizationName = "Galaxy Fabricators",
                                OrganizationSize = 190,
                                OrganizationWeb = "https://www.galaxyfabricators.com",
                                IndustryID = 8 // Related to "Galaxy Metals"
                            },
                            new Organization
                            {
                                ID = 9,
                                OrganizationName = "Ironclad Enterprises",
                                OrganizationSize = 400,
                                OrganizationWeb = "https://www.ironcladent.com",
                                IndustryID = 9 // Related to "Ironclad Industries"
                            },
                            new Organization
                            {
                                ID = 10,
                                OrganizationName = "Silverline Engineering",
                                OrganizationSize = 220,
                                OrganizationWeb = "https://www.silverlineeng.com",
                                IndustryID = 10 // Related to "Silverline Fabrication"
                            },
                            new Organization
                            {
                                ID = 11,
                                OrganizationName = "Star Steel Corp.",
                                OrganizationSize = 500,
                                OrganizationWeb = "https://www.starsteelcorp.com",
                                IndustryID = 11 // Related to "Star Steelworks"
                            },
                            new Organization
                            {
                                ID = 12,
                                OrganizationName = "Summit Iron & Steel",
                                OrganizationSize = 350,
                                OrganizationWeb = "https://www.summitironsteel.com",
                                IndustryID = 12 // Related to "Summit Metal Co."
                            },
                            new Organization
                            {
                                ID = 13,
                                OrganizationName = "Everest Metal Industries",
                                OrganizationSize = 280,
                                OrganizationWeb = "https://www.everestmetals.com",
                                IndustryID = 13 // Related to "Everest Iron Corp."
                            },
                            new Organization
                            {
                                ID = 14,
                                OrganizationName = "Prime Alloy Co.",
                                OrganizationSize = 330,
                                OrganizationWeb = "https://www.primealloy.com",
                                IndustryID = 14 // Related to "Prime Alloy Coatings"
                            },
                            new Organization
                            {
                                ID = 15,
                                OrganizationName = "Magnum Steelworks",
                                OrganizationSize = 420,
                                OrganizationWeb = "https://www.magnumsteelworks.com",
                                IndustryID = 15 // Related to "Magnum Steel Solutions"
                            }
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
                                TypeDescr = "Discounted membership for students with valid ID."
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
                                MemberName = "John Doe",
                                JoinDate = new DateTime(2021, 6, 15),
                                StandingStatus = StandingStatus.Good,
                                OrganizationID = 1, // Linked to "Steel Works Ltd."
                                Address = new Address
                                {
                                    AddressLineOne = "123 Elm St",
                                    AddressLineTwo = "Apt 4B",
                                    City = "Metaltown",
                                    StateProvince = "Metal State",
                                    PostalCode = "M1E2L3",
                                    Country = "USA"
                                }
                            },
                            new Member
                            {
                                ID = 2,
                                MemberName = "Jane Smith",
                                JoinDate = new DateTime(2022, 1, 20),
                                StandingStatus = StandingStatus.Inactive,
                                OrganizationID = 2, // Linked to "TISCO Technologies"
                                Address = new Address
                                {
                                    AddressLineOne = "456 Oak Ave",
                                    AddressLineTwo = null,
                                    City = "Ironville",
                                    StateProvince = "Steel Province",
                                    PostalCode = "I2R3O4",
                                    Country = "Canada"
                                }
                            },
                            new Member
                            {
                                ID = 3,
                                MemberName = "Robert Johnson",
                                JoinDate = new DateTime(2023, 3, 12),
                                StandingStatus = StandingStatus.Cancelled,
                                OrganizationID = 3, // Linked to "Iron Builders Inc."
                                Address = new Address
                                {
                                    AddressLineOne = "789 Pine Rd",
                                    AddressLineTwo = "Suite 302",
                                    City = "Steelton",
                                    StateProvince = "Metal Zone",
                                    PostalCode = "S3T4E5",
                                    Country = "UK"
                                }
                            },
                            new Member
                            {
                                ID = 4,
                                MemberName = "Emily Davis",
                                JoinDate = new DateTime(2022, 8, 8),
                                StandingStatus = StandingStatus.Good,
                                OrganizationID = 4, // Linked to "Forge Foundry Co."
                                Address = new Address
                                {
                                    AddressLineOne = "101 Birch Blvd",
                                    AddressLineTwo = null,
                                    City = "Alloy City",
                                    StateProvince = "Forge Region",
                                    PostalCode = "A1C2T3",
                                    Country = "USA"
                                }
                            }
                        );
                        context.SaveChanges();
                    }

                    if (!context.MemberMembershipTypes.Any())
                    {
                        context.MemberMembershipTypes.AddRange(
                            new MemberMembershipType
                            {
                                MemberID = 1, // John Doe
                                MembershipTypeID = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 1, // John Doe
                                MembershipTypeID = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 2, // Jane Smith
                                MembershipTypeID = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 3, // Robert Johnson
                                MembershipTypeID = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 4, // Emily Davis
                                MembershipTypeID = 1 // Basic Membership
                            }
                        );
                        context.SaveChanges();
                    }
                    if (!context.Contacts.Any())
                    {
                        context.Contacts.AddRange(
                            new Contact
                            {
                                ContactName = "John Doe",
                                Title = "Manager",
                                Department = "Sales",
                                EMail = "john.doe@example.com",
                                Phone = "1234567890",
                                LinkedinUrl = "https://www.linkedin.com/in/johndoe",
                                IsVIP = true
                            },
                            new Contact
                            {
                                ContactName = "Jane Smith",
                                Title = "Director",
                                Department = "Marketing",
                                EMail = "jane.smith@example.com",
                                Phone = "9876543210",
                                LinkedinUrl = "https://www.linkedin.com/in/janesmith",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ContactName = "Alice Johnson",
                                Title = "VP",
                                Department = "Human Resources",
                                EMail = "alice.johnson@example.com",
                                Phone = "5551234567",
                                LinkedinUrl = "https://www.linkedin.com/in/alicejohnson",
                                IsVIP = true
                            },
                            new Contact
                            {
                                ContactName = "Bob Brown",
                                Title = "Chief Financial Officer",
                                Department = "Finance",
                                EMail = "bob.brown@example.com",
                                Phone = "5557654321",
                                LinkedinUrl = "https://www.linkedin.com/in/bobbrown",
                                IsVIP = true
                            },
                            new Contact
                            {
                                ContactName = "Charlie Davis",
                                Title = "Chief Operating Officer",
                                Department = "Operations",
                                EMail = "charlie.davis@example.com",
                                Phone = "5557890123",
                                LinkedinUrl = "https://www.linkedin.com/in/charliedavis",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ContactName = "Deborah Williams",
                                Title = "Director of Technology",
                                Department = "Technology",
                                EMail = "deborah.williams@example.com",
                                Phone = "5552345678",
                                LinkedinUrl = "https://www.linkedin.com/in/deborahwilliams",
                                IsVIP = true
                            },
                            new Contact
                            {
                                ContactName = "Eve Taylor",
                                Title = "Marketing Specialist",
                                Department = "Marketing",
                                EMail = "eve.taylor@example.com",
                                Phone = "5553456789",
                                LinkedinUrl = "https://www.linkedin.com/in/evetaylor",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ContactName = "Frank Harris",
                                Title = "Senior Engineer",
                                Department = "Engineering",
                                EMail = "frank.harris@example.com",
                                Phone = "5554567890",
                                LinkedinUrl = "https://www.linkedin.com/in/frankharris",
                                IsVIP = true
                            },
                            new Contact
                            {
                                ContactName = "Grace King",
                                Title = "Business Development Manager",
                                Department = "Sales",
                                EMail = "grace.king@example.com",
                                Phone = "5555678901",
                                LinkedinUrl = "https://www.linkedin.com/in/graceking",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ContactName = "Hank Lee",
                                Title = "Head of Research",
                                Department = "Research and Development",
                                EMail = "hank.lee@example.com",
                                Phone = "5556789012",
                                LinkedinUrl = "https://www.linkedin.com/in/hanklee",
                                IsVIP = true
                            },
                            new Contact
                            {
                                ContactName = "Ivy Adams",
                                Title = "Project Manager",
                                Department = "Operations",
                                EMail = "ivy.adams@example.com",
                                Phone = "5557890123",
                                LinkedinUrl = "https://www.linkedin.com/in/ivyadams",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ContactName = "Jack Scott",
                                Title = "CEO",
                                Department = "Executive",
                                EMail = "jack.scott@example.com",
                                Phone = "5558901234",
                                LinkedinUrl = "https://www.linkedin.com/in/jackscott",
                                IsVIP = true
                            },
                            new Contact
                            {
                                ContactName = "Kathy Morris",
                                Title = "HR Specialist",
                                Department = "Human Resources",
                                EMail = "kathy.morris@example.com",
                                Phone = "5559012345",
                                LinkedinUrl = "https://www.linkedin.com/in/kathymorris",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ContactName = "Louis Walker",
                                Title = "Customer Service Lead",
                                Department = "Customer Service",
                                EMail = "louis.walker@example.com",
                                Phone = "5550123456",
                                LinkedinUrl = "https://www.linkedin.com/in/louiswalker",
                                IsVIP = true
                            },
                            new Contact
                            {
                                ContactName = "Mona White",
                                Title = "Legal Advisor",
                                Department = "Legal",
                                EMail = "mona.white@example.com",
                                Phone = "5551234567",
                                LinkedinUrl = "https://www.linkedin.com/in/monawhite",
                                IsVIP = false
                            }
                        );
                        context.SaveChanges();
                    }

                    if (!context.ContactOrganizations.Any())
                    {
                        context.ContactOrganizations.AddRange(
                            new ContactOrganization { ContactID = 1, OrganizationID = 1 },
                            new ContactOrganization { ContactID = 2, OrganizationID = 2 },
                            new ContactOrganization { ContactID = 3, OrganizationID = 3 },
                            new ContactOrganization { ContactID = 4, OrganizationID = 4 },
                            new ContactOrganization { ContactID = 5, OrganizationID = 5 },
                            new ContactOrganization { ContactID = 6, OrganizationID = 6 },
                            new ContactOrganization { ContactID = 7, OrganizationID = 7 },
                            new ContactOrganization { ContactID = 8, OrganizationID = 8 },
                            new ContactOrganization { ContactID = 9, OrganizationID = 9 },
                            new ContactOrganization { ContactID = 10, OrganizationID = 10 },
                            new ContactOrganization { ContactID = 11, OrganizationID = 11 },
                            new ContactOrganization { ContactID = 12, OrganizationID = 12 },
                            new ContactOrganization { ContactID = 13, OrganizationID = 13 },
                            new ContactOrganization { ContactID = 14, OrganizationID = 14 },
                            new ContactOrganization { ContactID = 15, OrganizationID = 15 }
                        );
                        context.SaveChanges();
                    }
                    if (!context.Opportunities.Any())
                    {
                        context.Opportunities.AddRange(
                            new Opportunity
                            {
                                OpportunityName = "New Partnership with TechCo",
                                OpportunityDescr = "Potential collaboration with TechCo to offer joint solutions.",
                                OpportunityStatus = OpportunityStatus.Open,
                                OrganizationID = 1,  // Assuming Organization with ID 1 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 1, 5),
                                //    InteractionNote = "Initial discussion on potential partnership."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Software Development for HealthCorp",
                                OpportunityDescr = "Software development project for HealthCorp to enhance their internal systems.",
                                OpportunityStatus = OpportunityStatus.InProgress,
                                OrganizationID = 2,  // Assuming Organization with ID 2 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 1, 10),
                                //    InteractionNote = "Meeting to finalize project requirements."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Website Overhaul for FinServe",
                                OpportunityDescr = "Website redesign project for FinServe to improve their online presence.",
                                OpportunityStatus = OpportunityStatus.Closed,
                                OrganizationID = 3,  // Assuming Organization with ID 3 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 1, 15),
                                //    InteractionNote = "Final meeting to close project details."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Mobile App Development for EduTech",
                                OpportunityDescr = "Development of a mobile app for EduTech to expand their reach in the education sector.",
                                OpportunityStatus = OpportunityStatus.Open,
                                OrganizationID = 4,  // Assuming Organization with ID 4 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 1, 18),
                                //    InteractionNote = "Kickoff meeting for mobile app project."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "CRM System for SalesForce",
                                OpportunityDescr = "Implementation of a CRM system for SalesForce to improve their customer relationship management.",
                                OpportunityStatus = OpportunityStatus.InProgress,
                                OrganizationID = 5,  // Assuming Organization with ID 5 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 1, 20),
                                //    InteractionNote = "Meeting to discuss CRM system features."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "E-commerce Platform for ShopMart",
                                OpportunityDescr = "Development of a full-fledged e-commerce platform for ShopMart.",
                                OpportunityStatus = OpportunityStatus.Closed,
                                OrganizationID = 6,  // Assuming Organization with ID 6 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 1, 22),
                                //    InteractionNote = "Final review meeting before project closure."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "AI Integration for SmartTech",
                                OpportunityDescr = "Integration of AI-based solutions for SmartTech's systems.",
                                OpportunityStatus = OpportunityStatus.Open,
                                OrganizationID = 7,  // Assuming Organization with ID 7 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 1, 25),
                                //    InteractionNote = "Discussion about AI integration and scope."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Cloud Migration for DataCloud",
                                OpportunityDescr = "Cloud migration for DataCloud to streamline their operations and storage.",
                                OpportunityStatus = OpportunityStatus.InProgress,
                                OrganizationID = 8,  // Assuming Organization with ID 8 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 1, 28),
                                //    InteractionNote = "Discussing cloud architecture for migration."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Cybersecurity for SecureNet",
                                OpportunityDescr = "Cybersecurity services for SecureNet to enhance their data protection.",
                                OpportunityStatus = OpportunityStatus.Closed,
                                OrganizationID = 9,  // Assuming Organization with ID 9 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 1, 30),
                                //    InteractionNote = "Final agreement on cybersecurity solutions."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Data Analytics for AnalyticsPro",
                                OpportunityDescr = "Implementing a data analytics platform for AnalyticsPro to improve decision-making.",
                                OpportunityStatus = OpportunityStatus.Open,
                                OrganizationID = 10,  // Assuming Organization with ID 10 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 2, 2),
                                //    InteractionNote = "Initial discussion on data analytics requirements."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Business Intelligence for BizIntel",
                                OpportunityDescr = "Providing business intelligence solutions for BizIntel to enhance reporting capabilities.",
                                OpportunityStatus = OpportunityStatus.InProgress,
                                OrganizationID = 11,  // Assuming Organization with ID 11 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 2, 5),
                                //    InteractionNote = "Meeting to review BI system features."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "ERP System for GlobalCorp",
                                OpportunityDescr = "ERP system implementation for GlobalCorp to streamline operations.",
                                OpportunityStatus = OpportunityStatus.Closed,
                                OrganizationID = 12,  // Assuming Organization with ID 12 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 2, 7),
                                //    InteractionNote = "Reviewing implementation plan and timeline."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Virtual Events Platform for EventPro",
                                OpportunityDescr = "Development of a platform for virtual events for EventPro.",
                                OpportunityStatus = OpportunityStatus.Open,
                                OrganizationID = 13,  // Assuming Organization with ID 13 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 2, 10),
                                //    InteractionNote = "Kickoff meeting to discuss platform features."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Logistics Management System for MoveIt",
                                OpportunityDescr = "Logistics management software for MoveIt to optimize their operations.",
                                OpportunityStatus = OpportunityStatus.InProgress,
                                OrganizationID = 14,  // Assuming Organization with ID 14 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 2, 12),
                                //    InteractionNote = "Discussing project scope and logistics software needs."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Blockchain Solutions for ChainTech",
                                OpportunityDescr = "Developing blockchain-based solutions for ChainTech's supply chain management.",
                                OpportunityStatus = OpportunityStatus.Closed,
                                OrganizationID = 15,  // Assuming Organization with ID 15 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 2, 14),
                                //    InteractionNote = "Final meeting on blockchain integration."
                                //}
                            }
                        );
                        context.SaveChanges();
                    }

                    if (!context.Interactions.Any())
                    {
                        context.Interactions.AddRange(
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 1, 1),
                                InteractionNote = "Initial contact for potential collaboration.",
                                ContactID = 1,
                                MemberID = 1,
                                OpportunityID = 1
                            },
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 1, 3),
                                InteractionNote = "Follow-up email regarding partnership details.",
                                ContactID = 2,
                                MemberID = 2,
                                OpportunityID = 2
                            },
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 1, 5),
                                InteractionNote = "Scheduled call to discuss project needs.",
                                ContactID = 3,
                                MemberID = 3,
                                OpportunityID = 3
                            },
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 1, 10),
                                InteractionNote = "Met to discuss contract terms and conditions.",
                                ContactID = 4,
                                MemberID = 4,
                                OpportunityID = 4
                            }
                        //    new Interaction
                        //    {
                        //        InteractionDate = new DateTime(2025, 1, 12),
                        //        InteractionNote = "Client inquiry on pricing models.",
                        //        ContactID = 5,
                        //        MemberID = 1,
                        //        OpportunityID = 5
                        //    },
                        //    new Interaction
                        //    {
                        //        InteractionDate = new DateTime(2025, 1, 15),
                        //        InteractionNote = "Discussed solution packages for enterprise clients.",
                        //        ContactID = 6,
                        //        MemberID = 2,
                        //        OpportunityID = 6
                        //    },
                        //    new Interaction
                        //    {
                        //        InteractionDate = new DateTime(2025, 1, 18),
                        //        InteractionNote = "Following up on service proposal.",
                        //        ContactID = 7,
                        //        MemberID = 3,
                        //        OpportunityID = 7
                        //    },
                        //    new Interaction
                        //    {
                        //        InteractionDate = new DateTime(2025, 1, 20),
                        //        InteractionNote = "Finalizing service agreement terms.",
                        //        ContactID = 8,
                        //        MemberID = 4,
                        //        OpportunityID = 8
                        //    },
                        //    new Interaction
                        //    {
                        //        InteractionDate = new DateTime(2025, 1, 22),
                        //        InteractionNote = "Agreement on next steps and deliverables.",
                        //        ContactID = 9,
                        //        MemberID = 1,
                        //        OpportunityID = 9
                        //    },
                        //    new Interaction
                        //    {
                        //        InteractionDate = new DateTime(2025, 1, 25),
                        //        InteractionNote = "Reviewing deliverables for upcoming project.",
                        //        ContactID = 10,
                        //        MemberID = 2,
                        //        OpportunityID = 10
                        //    },
                        //    new Interaction
                        //    {
                        //        InteractionDate = new DateTime(2025, 1, 28),
                        //        InteractionNote = "Update on progress and timeline.",
                        //        ContactID = 11,
                        //        MemberID = 3,
                        //        OpportunityID = 11
                        //    },
                        //    new Interaction
                        //    {
                        //        InteractionDate = new DateTime(2025, 1, 30),
                        //        InteractionNote = "Follow-up on final proposal details.",
                        //        ContactID = 12,
                        //        MemberID = 4,
                        //        OpportunityID = 12
                        //    },
                        //    new Interaction
                        //    {
                        //        InteractionDate = new DateTime(2025, 2, 2),
                        //        InteractionNote = "Final meeting before project launch.",
                        //        ContactID = 13,
                        //        MemberID = 1,
                        //        OpportunityID = 13
                        //    },
                        //    new Interaction
                        //    {
                        //        InteractionDate = new DateTime(2025, 2, 5),
                        //        InteractionNote = "Confirming details of service agreement.",
                        //        ContactID = 14,
                        //        MemberID = 2,
                        //        OpportunityID = 14
                        //    },
                        //    new Interaction
                        //    {
                        //        InteractionDate = new DateTime(2025, 2, 7),
                        //        InteractionNote = "Meeting to finalize documentation.",
                        //        ContactID = 3,
                        //        MemberID = 3,
                        //        OpportunityID = 15
                        //    }
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
                                MemberID = 1  // Assuming Member with ID 1 exists
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 15),
                                Canceled = true,
                                CancellationNote = "Member canceled their subscription after failing to make payments.",
                                MemberID = 2  // Assuming Member with ID 2 exists
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
                            }
                            //new Cancellation
                            //{
                            //    CancellationDate = new DateTime(2025, 1, 18),
                            //    Canceled = false,
                            //    CancellationNote = "Active member, no cancellation.",
                            //    MemberID = 5
                            //},
                            //new Cancellation
                            //{
                            //    CancellationDate = new DateTime(2025, 1, 20),
                            //    Canceled = false,
                            //    CancellationNote = "Active member, no cancellation.",
                            //    MemberID = 6
                            //},
                            //new Cancellation
                            //{
                            //    CancellationDate = new DateTime(2025, 1, 22),
                            //    Canceled = false,
                            //    CancellationNote = "Active member, no cancellation.",
                            //    MemberID = 7
                            //},
                            //new Cancellation
                            //{
                            //    CancellationDate = new DateTime(2025, 1, 25),
                            //    Canceled = false,
                            //    CancellationNote = "Active member, no cancellation.",
                            //    MemberID = 8
                            //},
                            //new Cancellation
                            //{
                            //    CancellationDate = new DateTime(2025, 1, 27),
                            //    Canceled = false,
                            //    CancellationNote = "Active member, no cancellation.",
                            //    MemberID = 9
                            //},
                            //new Cancellation
                            //{
                            //    CancellationDate = new DateTime(2025, 1, 30),
                            //    Canceled = false,
                            //    CancellationNote = "Active member, no cancellation.",
                            //    MemberID = 10
                            //},
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
                    }
                }

                catch { }
                #endregion
            }

        }
    }
}
