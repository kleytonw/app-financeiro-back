namespace ERP.Models.New
{
    public class FilterPersonModel
    {
        public int IdPerson { get; set; }
        public string Name { get; set; }
    }

    public class SearchPersonModel
    {
        public string TipoBusca { get; set; }
        public string ValorBusca { get; set; }

        public string Type { get; set; }
    }

    public class ResponsePersonModel
    {
        public int IdPerson { get; set; }
        public string DisplayName { get; set; }
        public string Company { get; set; }
        public string Phone2 { get; set; }
        public string Phone1 { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
    }


    public class RequestPersonModel
    {
        public int IdPerson { get; set; }
        public string FirstName { get; set; }
        public string MidleName { get; set; }
        public string Company { get; set; }
        public string DisplayName { get; set; }

        // Contacts
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }


        // Address
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Province { get; set; }

        public string Type { get; set; } // customer // dealler etc... 

        public string Description { get; set;  }
    }

    public class PersonModel
    {
        public int IdPerson { get; set; }
        public string FirstName { get; set; }
        public string MidleName { get; set; }
        public string Company { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        // Contacts
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }

        // Address
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Province { get; set; }
        public string Type { get; set; } // customer // dealler etc... 
    }


}
