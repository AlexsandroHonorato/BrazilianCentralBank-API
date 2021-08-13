using System;
using System.Collections.Generic;
using System.Text;

namespace BrazilianCentralBank.Infrastructure.Data.Interfaces {
    public interface IRepositoryBase {
        string ConnectionString { get; }
    }
}
