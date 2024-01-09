using ELMSApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace ELMSApplication
{
    public class ELMSApplicationContext : DbContext
    {
        public ELMSApplicationContext(DbContextOptions<ELMSApplicationContext>options):base(options){

        }
       
        public DbSet<Leave> Leave { get; set; }   

         public DbSet<Employee> Employee { get; set; }   

    }
}