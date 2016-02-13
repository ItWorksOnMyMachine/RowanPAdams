using Repository.Pattern.Repositories;
using RowanAdams.BusinessObjects;
using RowanAdams.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RowanAdams.Queries
{
    public static class LogEntryQueries
    {
        public static IEnumerable<ChoreLogEntryBO> GetLogEntriesWithChores(this IRepository<LogEntry> repository)
        {
            // This is the simple nav property way.
            //return repository.Queryable().Select(le => new ChoreLogEntryBO
            //{
            //    CompletedDate = le.CompletedDate,
            //    Value = le.Chore.Value,
            //    Name = le.Chore.Name
            //});

            // And if you need better control over the query you can do this.
            var logEntriesQuery = repository.GetRepository<LogEntry>().Queryable();
            var choresQuery = repository.GetRepository<Chore>().Queryable();

            return logEntriesQuery
                .Join(choresQuery, le => le.ChoreId, c => c.Id, (le, c) => new ChoreLogEntryBO
                {
                    CompletedDate = le.CompletedDate,
                    Value = c.Value,
                    Name = c.Name
                });
        }
    }
}
