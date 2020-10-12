using System;
using System.Collections.Generic;
using System.Text;

namespace SmileBot.Core.Services.Database
{
    public interface IUnitOfWork
    {
        public SmileContext _context { get; }
    }
}