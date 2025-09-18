using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using System;          
using System.Collections.Generic;
using System.Linq;     

namespace Grocery.Core.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly List<Client> _clients;

        public ClientRepository()
        {
            _clients = new()
            {
                new Client(1, "I.H. Visser",   "user1@mail.com", "IEvwJmu+qsyJaM47ZeP0bQ==.1O+f+olavx1V99HZcktHopsXIxfo5IrDj0m5WyY5mmM="),
                new Client(2, "H.H. Hermans", "user2@mail.com", "dOk+X+wt+MA9uIniRGKDFg==.QLvy72hdG8nWj1FyL75KoKeu4DUgu5B/HAHqTD2UFLU="),
                new Client(3, "A.J. Kwak",    "user3@mail.com", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA="),
            };
        }
        public Client? Get(string email) => _clients.FirstOrDefault(c => 
            string.Equals(c.Email, email, StringComparison.OrdinalIgnoreCase));

        public Client? Get(int id) => _clients.FirstOrDefault(c => c.Id == id);

        public List<Client> GetAll() => _clients.ToList();
    }
}
