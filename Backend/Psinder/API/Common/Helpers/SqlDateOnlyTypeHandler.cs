﻿using Dapper;
using System.Data;

namespace Psinder.API.Common.Helpers;

public class SqlDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly date)
    {
        parameter.Value = date.ToDateTime(new TimeOnly(0, 0));
    }

    public override DateOnly Parse(object value)
    {
        return DateOnly.FromDateTime((DateTime)value);
    }
}