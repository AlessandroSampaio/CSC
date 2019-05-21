﻿// <auto-generated />
using System;
using CSC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CSC.Migrations
{
    [DbContext(typeof(CSCContext))]
    [Migration("20190521105219_ClienteMono")]
    partial class ClienteMono
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CSC.Models.Atendimento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Abertura");

                    b.Property<int>("AtendimentoTipo");

                    b.Property<int>("ClienteId");

                    b.Property<string>("Detalhes");

                    b.Property<DateTime>("Encerramento");

                    b.Property<int>("FuncionarioId");

                    b.Property<int?>("OrigemID");

                    b.Property<string>("Solicitante");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.HasIndex("FuncionarioId");

                    b.ToTable("Atendimento");
                });

            modelBuilder.Entity("CSC.Models.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bairro");

                    b.Property<string>("CEP");

                    b.Property<string>("CNPJ")
                        .IsRequired();

                    b.Property<string>("Cidade");

                    b.Property<DateTime>("DataInicio");

                    b.Property<string>("Email");

                    b.Property<string>("Logradouro");

                    b.Property<bool>("Mono");

                    b.Property<string>("NomeFantasia")
                        .IsRequired();

                    b.Property<int?>("Numero");

                    b.Property<string>("RazaoSocial")
                        .IsRequired();

                    b.Property<int>("Status");

                    b.Property<string>("Telefone");

                    b.HasKey("Id");

                    b.ToTable("Cliente");
                });

            modelBuilder.Entity("CSC.Models.Funcionario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Admissao");

                    b.Property<DateTime?>("Demissao");

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.Property<bool>("Veiculo");

                    b.HasKey("Id");

                    b.ToTable("Funcionario");
                });

            modelBuilder.Entity("CSC.Models.Inventario", b =>
                {
                    b.Property<int>("ClienteID");

                    b.Property<int>("Software");

                    b.Property<int>("Quantidade");

                    b.HasKey("ClienteID", "Software");

                    b.ToTable("Inventario");
                });

            modelBuilder.Entity("CSC.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FuncionarioId");

                    b.Property<string>("NomeLogon")
                        .IsRequired();

                    b.Property<string>("Senha")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("FuncionarioId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("CSC.Models.Atendimento", b =>
                {
                    b.HasOne("CSC.Models.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CSC.Models.Funcionario", "Funcionario")
                        .WithMany()
                        .HasForeignKey("FuncionarioId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CSC.Models.Inventario", b =>
                {
                    b.HasOne("CSC.Models.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CSC.Models.User", b =>
                {
                    b.HasOne("CSC.Models.Funcionario", "Funcionario")
                        .WithMany()
                        .HasForeignKey("FuncionarioId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
