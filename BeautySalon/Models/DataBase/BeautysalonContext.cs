﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Models.DataBase;

public partial class BeautysalonContext : DbContext
{
    public BeautysalonContext()
    {
    }

    public BeautysalonContext(DbContextOptions<BeautysalonContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bitacora> Bitacoras { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EstadoReserva> EstadoReservas { get; set; }

    public virtual DbSet<Holiday> Holidays { get; set; }

    public virtual DbSet<HoursAvailable> HoursAvailables { get; set; }

    public virtual DbSet<InformacionDePago> InformacionDePagos { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    public virtual DbSet<Membresium> Membresia { get; set; }

    public virtual DbSet<NivelBitacora> NivelBitacoras { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<RoleEmployee> RoleEmployees { get; set; }

    public virtual DbSet<ServiceDetail> ServiceDetails { get; set; }

    public virtual DbSet<Tax> Taxes { get; set; }

    public virtual DbSet<Timetable> Timetables { get; set; }

    public virtual DbSet<TipoPago> TipoPagos { get; set; }

    public virtual DbSet<TypeUser> TypeUsers { get; set; }

    public virtual DbSet<UserAdmin> UserAdmins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=beautysalon.c6boib1qjmh9.us-east-1.rds.amazonaws.com; user=sa; password=salon123; database=beautysalon; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bitacora>(entity =>
        {
            entity.HasKey(e => e.IdBitacora).HasName("PK__Bitacora__223FE14209C4F919");

            entity.ToTable("Bitacora");

            entity.Property(e => e.IdBitacora).HasColumnName("idBitacora");
            entity.Property(e => e.Descripcion)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.DetallesAdicionales)
                .IsUnicode(false)
                .HasColumnName("detallesAdicionales");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.Nivel).HasColumnName("nivel");
            entity.Property(e => e.Usuario).HasColumnName("usuario");

            entity.HasOne(d => d.NivelNavigation).WithMany(p => p.Bitacoras)
                .HasForeignKey(d => d.Nivel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bitacora_nivel");

            entity.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Bitacoras)
                .HasForeignKey(d => d.Usuario)
                .HasConstraintName("fk_bitacora_user");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("PK__Category__79D361B60C6EF548");

            entity.ToTable("Category");

            entity.Property(e => e.IdCategory).HasColumnName("idCategory");
            entity.Property(e => e.Category1)
                .HasMaxLength(50)
                .HasColumnName("category");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.IdCustomer).HasName("PK__Customer__D058768620D76DCC");

            entity.ToTable("Customer");

            entity.Property(e => e.IdCustomer).HasColumnName("idCustomer");
            entity.Property(e => e.CreateDate)
                .HasColumnType("date")
                .HasColumnName("createDate");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(40)
                .HasColumnName("fullName");
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Customers)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("fk_customer_user");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.IdEmployee).HasName("PK__Employee__227F26A551C9AF0A");

            entity.ToTable("Employee");

            entity.Property(e => e.IdEmployee).HasColumnName("idEmployee");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.DateCreate)
                .HasColumnType("date")
                .HasColumnName("dateCreate");
            entity.Property(e => e.DateModify)
                .HasColumnType("date")
                .HasColumnName("dateModify");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("date")
                .HasColumnName("dateOfBirth");
            entity.Property(e => e.Dni)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DNI");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeActive).HasColumnName("employeeActive");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("firstName");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.IdRole).HasColumnName("idRole");
            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lastName");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_role_employee");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_employee");
        });

        modelBuilder.Entity<EstadoReserva>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__EstadoRe__62EA894A5900E2D7");

            entity.ToTable("EstadoReserva");

            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Holiday>(entity =>
        {
            entity.HasKey(e => e.IdHoliday).HasName("PK__Holiday__2A86D697F2133C29");

            entity.ToTable("Holiday");

            entity.Property(e => e.IdHoliday).HasColumnName("idHoliday");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("_date");
            entity.Property(e => e.Reason)
                .IsUnicode(false)
                .HasColumnName("reason");
        });

        modelBuilder.Entity<HoursAvailable>(entity =>
        {
            entity.HasKey(e => e.IdHour).HasName("PK__HoursAva__777963AEE0AFD541");

            entity.ToTable("HoursAvailable");

            entity.Property(e => e.IdHour).HasColumnName("idHour");
            entity.Property(e => e.Hour)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("_hour");
        });

        modelBuilder.Entity<InformacionDePago>(entity =>
        {
            entity.HasKey(e => e.IdInfoPago).HasName("PK__Informac__191B44ABE4604766");

            entity.ToTable("InformacionDePago");

            entity.Property(e => e.IdInfoPago).HasColumnName("idInfoPago");
            entity.Property(e => e.Ccv)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("ccv");
            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.NumeroTarjeta)
                .HasMaxLength(19)
                .IsUnicode(false)
                .HasColumnName("numeroTarjeta");
            entity.Property(e => e.Vence)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("vence");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.InformacionDePagos)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_infoPago_user");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.IdInvoice).HasName("PK__Invoice__D3631FCED67E7C65");

            entity.ToTable("Invoice");

            entity.Property(e => e.IdInvoice).HasColumnName("idInvoice");
            entity.Property(e => e.DateInvoice)
                .HasColumnType("date")
                .HasColumnName("dateInvoice");
            entity.Property(e => e.Discount)
                .HasColumnType("money")
                .HasColumnName("discount");
            entity.Property(e => e.IdCustomer).HasColumnName("idCustomer");
            entity.Property(e => e.IdPago).HasColumnName("idPago");
            entity.Property(e => e.NameCustomer)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nameCustomer");
            entity.Property(e => e.ReferencesNumber)
                .HasMaxLength(20)
                .HasColumnName("referencesNumber");
            entity.Property(e => e.Subtotal)
                .HasColumnType("money")
                .HasColumnName("subtotal");
            entity.Property(e => e.Tax)
                .HasColumnType("money")
                .HasColumnName("tax");
            entity.Property(e => e.Total)
                .HasColumnType("money")
                .HasColumnName("total");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.IdCustomer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_customer_invoice");

            entity.HasOne(d => d.IdPagoNavigation).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.IdPago)
                .HasConstraintName("fk_factura_pago");
        });

        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__InvoiceD__3213E83FC81D29AD");

            entity.ToTable("InvoiceDetail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdInvoice).HasColumnName("idInvoice");
            entity.Property(e => e.IdProduct).HasColumnName("idProduct");
            entity.Property(e => e.IdTypeTax).HasColumnName("idTypeTax");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Tax).HasColumnName("tax");

            entity.HasOne(d => d.IdTypeTaxNavigation).WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.IdTypeTax)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tax_invoicedetail");
        });

        modelBuilder.Entity<Membresium>(entity =>
        {
            entity.HasKey(e => e.IdMembresia).HasName("PK__Membresi__BDDB80E93550ACD2");

            entity.Property(e => e.IdMembresia).HasColumnName("idMembresia");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaFin)
                .HasColumnType("datetime")
                .HasColumnName("fechaFin");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("fechaInicio");
            entity.Property(e => e.IdPago).HasColumnName("idPago");
            entity.Property(e => e.Precio)
                .HasColumnType("money")
                .HasColumnName("precio");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.IdPagoNavigation).WithMany(p => p.Membresia)
                .HasForeignKey(d => d.IdPago)
                .HasConstraintName("fk_menbresia_pago");

            entity.HasOne(d => d.User).WithMany(p => p.Membresia)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_membresia");
        });

        modelBuilder.Entity<NivelBitacora>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NivelBit__3213E83FC7E4F37B");

            entity.ToTable("NivelBitacora");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nivel)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("nivel");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PK__Pago__BD2295AD125A5752");

            entity.ToTable("Pago");

            entity.Property(e => e.IdPago).HasColumnName("idPago");
            entity.Property(e => e.Cambio)
                .HasColumnType("money")
                .HasColumnName("cambio");
            entity.Property(e => e.Ccv)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("ccv");
            entity.Property(e => e.IdTipoPago).HasColumnName("idTipoPago");
            entity.Property(e => e.Monto)
                .HasColumnType("money")
                .HasColumnName("monto");
            entity.Property(e => e.NumeroDeTarjeta)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numeroDeTarjeta");
            entity.Property(e => e.Recibido)
                .HasColumnType("money")
                .HasColumnName("recibido");
            entity.Property(e => e.Vence)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("vence");

            entity.HasOne(d => d.IdTipoPagoNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdTipoPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_pago_tipo");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PK__Product__5EEC79D14D48B196");

            entity.ToTable("Product");

            entity.Property(e => e.IdProduct).HasColumnName("idProduct");
            entity.Property(e => e.CreateDate)
                .HasColumnType("date")
                .HasColumnName("createDate");
            entity.Property(e => e.Description).HasColumnName("_description");
            entity.Property(e => e.Featured).HasColumnName("featured");
            entity.Property(e => e.IdCategory).HasColumnName("idCategory");
            entity.Property(e => e.IdSkill)
                .HasDefaultValueSql("((1))")
                .HasColumnName("idSkill");
            entity.Property(e => e.IdTax).HasColumnName("idTax");
            entity.Property(e => e.ImageProduct)
                .IsUnicode(false)
                .HasColumnName("imageProduct");
            entity.Property(e => e.ModifyDate)
                .HasColumnType("date")
                .HasColumnName("modifyDate");
            entity.Property(e => e.NameProduct)
                .HasMaxLength(50)
                .HasColumnName("nameProduct");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.Sku)
                .HasMaxLength(40)
                .HasColumnName("SKU");
            entity.Property(e => e.Stock).HasDefaultValueSql("((1))");
            entity.Property(e => e.StockMinimum).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_category_product");

            entity.HasOne(d => d.IdSkillNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdSkill)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Skill_product");

            entity.HasOne(d => d.IdTaxNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdTax)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tax_product");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReserva).HasName("PK__Reserva__94D104C81F95CD2B");

            entity.ToTable("Reserva");

            entity.Property(e => e.IdReserva).HasColumnName("idReserva");
            entity.Property(e => e.Fecha)
                .HasColumnType("date")
                .HasColumnName("fecha");
            entity.Property(e => e.IdCustomer).HasColumnName("idCustomer");
            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.IdHora).HasColumnName("idHora");
            entity.Property(e => e.IdServicio).HasColumnName("idServicio");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdCustomer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Reserva_User");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("fk_Reserva_Estado");

            entity.HasOne(d => d.IdHoraNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdHora)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Reserva_Hora");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Reserva_Servicio");
        });

        modelBuilder.Entity<RoleEmployee>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__RoleEmpl__E5045C542C5C382F");

            entity.ToTable("RoleEmployee");

            entity.Property(e => e.IdRole).HasColumnName("idRole");
            entity.Property(e => e.NameRole)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nameRole");
        });

        modelBuilder.Entity<ServiceDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceD__3213E83FB7F08F8B");

            entity.ToTable("ServiceDetail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdProduct).HasColumnName("idProduct");
            entity.Property(e => e.IdService).HasColumnName("idService");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ServiceDetailIdProductNavigations)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_servicedetail");

            entity.HasOne(d => d.IdServiceNavigation).WithMany(p => p.ServiceDetailIdServiceNavigations)
                .HasForeignKey(d => d.IdService)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_service_servicedetail");
        });

        modelBuilder.Entity<Tax>(entity =>
        {
            entity.HasKey(e => e.IdTax).HasName("PK__Tax__020FEDA9A03E69B5");

            entity.ToTable("Tax");

            entity.Property(e => e.IdTax).HasColumnName("idTax");
            entity.Property(e => e.DescriptionTax).HasColumnName("descriptionTax");
            entity.Property(e => e.Tax1).HasColumnName("tax");
            entity.Property(e => e.TypeTax)
                .HasMaxLength(5)
                .HasColumnName("typeTax");
        });

        modelBuilder.Entity<Timetable>(entity =>
        {
            entity.HasKey(e => e.IdTimetable).HasName("PK__Timetabl__5DE92C40571870A3");

            entity.ToTable("Timetable");

            entity.Property(e => e.IdTimetable).HasColumnName("idTimetable");
            entity.Property(e => e.CloseHour).HasColumnName("closeHour");
            entity.Property(e => e.Day)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("_day");
            entity.Property(e => e.IsHoliday).HasColumnName("isHoliday");
            entity.Property(e => e.OpenHour).HasColumnName("openHour");

            entity.HasOne(d => d.CloseHourNavigation).WithMany(p => p.TimetableCloseHourNavigations)
                .HasForeignKey(d => d.CloseHour)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_close_timetable");

            entity.HasOne(d => d.OpenHourNavigation).WithMany(p => p.TimetableOpenHourNavigations)
                .HasForeignKey(d => d.OpenHour)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_open_timetable");
        });

        modelBuilder.Entity<TipoPago>(entity =>
        {
            entity.HasKey(e => e.IdTipoPago).HasName("PK__TipoPago__AC5BA85BF16CB417");

            entity.ToTable("TipoPago");

            entity.Property(e => e.IdTipoPago).HasColumnName("idTipoPago");
            entity.Property(e => e.Tipo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<TypeUser>(entity =>
        {
            entity.HasKey(e => e.IdType).HasName("PK__TypeUser__4BB98BC668BCF1B4");

            entity.ToTable("TypeUser");

            entity.Property(e => e.IdType).HasColumnName("idType");
            entity.Property(e => e.TypeName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("typeName");
        });

        modelBuilder.Entity<UserAdmin>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__UserAdmi__3717C9826FEA4CDD");

            entity.ToTable("UserAdmin");

            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.IdType).HasColumnName("idType");
            entity.Property(e => e.TokenPush)
                .IsUnicode(false)
                .HasColumnName("tokenPush");
            entity.Property(e => e.UserActive).HasColumnName("userActive");
            entity.Property(e => e.UserDateCreate)
                .HasColumnType("date")
                .HasColumnName("userDateCreate");
            entity.Property(e => e.UserDateModify)
                .HasColumnType("date")
                .HasColumnName("userDateModify");
            entity.Property(e => e.UserName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("userName");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userPassword");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.UserAdmins)
                .HasForeignKey(d => d.IdType)
                .HasConstraintName("fk_user_type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
