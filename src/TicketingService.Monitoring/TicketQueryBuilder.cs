namespace TicketingService.Monitoring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Abstractions;
    using Marten;

    public enum Operators
    {
        WHERE, AND, OR
    }

    public class TicketQueryBuilder
    {
        private StringBuilder query;
        private readonly IDocumentStore _documentStore;

        private static DateTimeOffset DefaultFromDateFilter => DateTime.Now.AddMonths(-6);

        public TicketQueryBuilder(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
            query = new StringBuilder($"SELECT data " +
                                      $"FROM mt_doc_ticket ");
        }

        public TicketQueryBuilder WhitelistedRegistries(Operators op, params string[]? registries)
        {
            if (registries is null || !registries.Any())
            {
                return this;
            }

            query.Append($" {op.ToString()} ");
            query.Append("data -> 'metadata'->> 'registry' IN (");
            query.Append(string.Join(", ", registries.Select(r => $"'{r}'")));
            query.Append(')');

            return this;
        }

        public TicketQueryBuilder BlacklistedActions(Operators op, params string[]? actions)
        {
            if (actions is null || !actions.Any())
            {
                return this;
            }

            query.Append($" {op.ToString()} ");
            query.Append("data -> 'metadata'->> 'action' NOT IN (");
            query.Append(string.Join(", ", actions.Select(r => $"'{r}'")));
            query.Append(')');

            return this;
        }

        public TicketQueryBuilder FromTo(
            Operators op,
            DateTimeOffset fromDate,
            DateTimeOffset toDate)
        {
            var format = "yyyy-MM-ddTHH:mm:ss.fffffffzzz";

            query.Append($" {op.ToString()} ");
            query.Append(
                $"(data ->> 'created')::timestamp with time zone >= '{fromDate.ToString(format)}'::timestamp with time zone " +
                $"AND (data ->> 'created')::timestamp with time zone <= '{toDate.ToString(format)}'::timestamp with time zone ");
            return this;
        }

        public TicketQueryBuilder FromTo(
            Operators op,
            string? fromDate,
            string? toDate)
        {
            var (from, to) = CreateDateRange(fromDate, toDate);

            return FromTo(op, from, to);
        }

        public TicketQueryBuilder ByStatuses(Operators op, params TicketStatus[]? statuses)
        {
            if (statuses == null || !statuses.Any())
                return this;

            query.Append($" {op.ToString()} ");
            query.Append($"data ->> 'status' IN (");
                query.Append(string.Join(", ", statuses.Select(s => $"'{s}'")));
                query.Append(")");

            return this;
        }
        public TicketQueryBuilder Paged(int offset, int limit)
        {
            query.Append($"OFFSET {offset} LIMIT {limit}");
            return this;
        }

        public IReadOnlyList<Ticket> Execute()
        {
           return _documentStore.QuerySession().Query<Ticket>(query.ToString());
        }

        private static (DateTimeOffset from, DateTimeOffset to) CreateDateRange(string? fromDate, string? toDate)
        {
            var from = GetStartDate(fromDate);
            var to = GetEndDate(from, toDate);
            return (from, to);
        }

        private static DateTimeOffset GetStartDate(string? fromDate)
        {
            return DateTimeOffset.TryParse(fromDate, out var dateTime)
                ? dateTime
                : DefaultFromDateFilter;
        }

        private static DateTimeOffset GetEndDate(DateTimeOffset fromDate, string? toDate)
        {
            if (DateTimeOffset.TryParse(toDate, out var dateTime) && dateTime > fromDate)
            {
                return dateTime;
            }

            return DateTimeOffset.Now.Date;
        }
    }
}
