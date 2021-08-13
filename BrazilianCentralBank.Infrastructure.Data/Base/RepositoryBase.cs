using System;
using System.Collections.Generic;
using System.Text;

using BrazilianCentralBank.Infrastructure.Data.Interfaces;

namespace BrazilianCentralBank.Infrastructure.Data.Base {
    public class RepositoryBase : IRepositoryBase {
        private readonly string _connectionString;

        public string ConnectionString {
            get { return _connectionString; }
        }

        public RepositoryBase(String connectionString) {
            this._connectionString = connectionString;
        }        
    }
}
