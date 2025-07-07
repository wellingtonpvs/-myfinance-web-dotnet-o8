using Microsoft.EntityFrameworkCore;
using myfinance_web_netcore.Models;

namespace myfinance_web_netcore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<PlanoContas> PlanoContas { get; set; } = null!;
        public DbSet<Transacao> Transacoes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade PlanoContas
            modelBuilder.Entity<PlanoContas>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Codigo)
                    .IsRequired();

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasConversion<int>();

                entity.Property(e => e.DataCriacao)
                    .IsRequired()
                    .HasDefaultValueSql("datetime('now')");

                // Índice único para o código
                entity.HasIndex(e => e.Codigo)
                    .IsUnique()
                    .HasDatabaseName("IX_PlanoContas_Codigo");

                // Nome da tabela
                entity.ToTable("PlanoContas");
            });

            // Configuração da entidade Transacao
            modelBuilder.Entity<Transacao>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Historico)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Data)
                    .IsRequired();

                entity.Property(e => e.Valor)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.DataCriacao)
                    .IsRequired()
                    .HasDefaultValueSql("datetime('now')");

                // Relacionamento com PlanoContas
                entity.HasOne(e => e.PlanoContas)
                    .WithMany()
                    .HasForeignKey(e => e.PlanoContasId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Nome da tabela
                entity.ToTable("Transacoes");
            });

            // Dados iniciais (Seed)
            modelBuilder.Entity<PlanoContas>().HasData(
                new PlanoContas { Id = 1, Codigo = 1, Descricao = "Combustível", Tipo = TipoPlanoContas.Despesa, DataCriacao = DateTime.Now },
                new PlanoContas { Id = 2, Codigo = 2, Descricao = "Supermercado", Tipo = TipoPlanoContas.Despesa, DataCriacao = DateTime.Now },
                new PlanoContas { Id = 3, Codigo = 3, Descricao = "Alimentação", Tipo = TipoPlanoContas.Despesa, DataCriacao = DateTime.Now },
                new PlanoContas { Id = 4, Codigo = 4, Descricao = "IPTU", Tipo = TipoPlanoContas.Despesa, DataCriacao = DateTime.Now },
                new PlanoContas { Id = 5, Codigo = 5, Descricao = "IPVA", Tipo = TipoPlanoContas.Despesa, DataCriacao = DateTime.Now },
                new PlanoContas { Id = 6, Codigo = 6, Descricao = "Salário", Tipo = TipoPlanoContas.Receita, DataCriacao = DateTime.Now },
                new PlanoContas { Id = 7, Codigo = 7, Descricao = "Escola", Tipo = TipoPlanoContas.Despesa, DataCriacao = DateTime.Now },
                new PlanoContas { Id = 8, Codigo = 8, Descricao = "Financiamento de Carro", Tipo = TipoPlanoContas.Despesa, DataCriacao = DateTime.Now }
            );

            // Seed de Transações
            modelBuilder.Entity<Transacao>().HasData(
                new Transacao { Id = 1, Historico = "Supermercado", Data = new DateTime(2025, 7, 5, 13, 0, 0), PlanoContasId = 2, Valor = 450.00m, DataCriacao = DateTime.Now },
                new Transacao { Id = 2, Historico = "Jantar", Data = new DateTime(2025, 6, 23, 20, 30, 0), PlanoContasId = 3, Valor = 250.00m, DataCriacao = DateTime.Now },
                new Transacao { Id = 3, Historico = "Salário", Data = new DateTime(2025, 6, 10, 0, 0, 0), PlanoContasId = 6, Valor = 2000.00m, DataCriacao = DateTime.Now },
                new Transacao { Id = 4, Historico = "Mensalidade Escola", Data = new DateTime(2025, 7, 1, 8, 30, 0), PlanoContasId = 7, Valor = 870.00m, DataCriacao = DateTime.Now }
            );
        }
    }
}
