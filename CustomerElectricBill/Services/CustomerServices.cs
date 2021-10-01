using CustomerElectricBill.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerElectricBill.Services
{
    public class CustomerServices
    {
        private readonly CustomerRepository _customerRepository;
        public CustomerServices(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public IEnumerable<dynamic> GetAll()
        {
            return _customerRepository.GetAll();
        }
    }
}
