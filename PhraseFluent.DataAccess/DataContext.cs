using Microsoft.EntityFrameworkCore;

namespace PhraseFluent.DataAccess;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options);