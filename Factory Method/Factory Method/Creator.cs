using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory_Method
{
    abstract class Creator
    {
        Product product;
        public abstract Product FactoryMethod(string[] parametrs);
        public void AnOperation(string[] parametrs)
        {
            product = FactoryMethod(parametrs);
        }
        public Product GetProduct() { return product; }
    }
}
