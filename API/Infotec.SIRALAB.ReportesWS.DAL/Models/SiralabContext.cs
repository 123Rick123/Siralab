using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Infotec.SIRALAB.ReportesWS.DAL.Models
{
    public partial class SiralabContext : DbContext
    {
        public SiralabContext()
        {
            Database.SetCommandTimeout(300);
        }

        public SiralabContext(DbContextOptions<SiralabContext> options) : base(options)
        {
        }

        public virtual DbSet<Analisi> Analises { get; set; }
        public virtual DbSet<AnalisisColaDetalle> AnalisisColaDetalles { get; set; }
        public virtual DbSet<AnalisisColaEncabezado> AnalisisColaEncabezados { get; set; }
        public virtual DbSet<AnalisisFaltaPago> AnalisisFaltaPagos { get; set; }
        public virtual DbSet<AnalisisFirma> AnalisisFirmas { get; set; }
        public virtual DbSet<Anexo> Anexos { get; set; }
        public virtual DbSet<AnexoMedidor> AnexoMedidors { get; set; }
        public virtual DbSet<CatDiaInhabil> CatDiaInhabils { get; set; }
        public virtual DbSet<CatGrupoMetodo> CatGrupoMetodos { get; set; }
        public virtual DbSet<CatModulo> CatModulos { get; set; }
        public virtual DbSet<CatPagina> CatPaginas { get; set; }
        public virtual DbSet<CatPermisosRol> CatPermisosRols { get; set; }
        public virtual DbSet<CatSistema> CatSistemas { get; set; }
        public virtual DbSet<CatTipoContacto> CatTipoContactos { get; set; }
        public virtual DbSet<CatTipoCuerpoReceptor> CatTipoCuerpoReceptors { get; set; }
        public virtual DbSet<CatTipoDocumentoAnexo> CatTipoDocumentoAnexos { get; set; }
        public virtual DbSet<CatTipoDocumentoMuestra> CatTipoDocumentoMuestras { get; set; }
        public virtual DbSet<CatTipoSignatario> CatTipoSignatarios { get; set; }
        public virtual DbSet<CatUnidadMedidum> CatUnidadMedida { get; set; }
        public virtual DbSet<CatUsoAgua> CatUsoAguas { get; set; }
        public virtual DbSet<CatUsuarioRol> CatUsuarioRols { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<ClienteContacto> ClienteContactos { get; set; }
        public virtual DbSet<ColoniaTipo> ColoniaTipos { get; set; }
        public virtual DbSet<Colonium> Colonia { get; set; }
        public virtual DbSet<ConfiguracionGrupoMetodo> ConfiguracionGrupoMetodos { get; set; }
        public virtual DbSet<Consecion> Consecions { get; set; }
        public virtual DbSet<Contacto> Contactos { get; set; }
        public virtual DbSet<Correspondencia_CR_UA_SIRFIC_SIRALAB> Correspondencia_CR_UA_SIRFIC_SIRALABs { get; set; }
        public virtual DbSet<CuerpoReceptor> CuerpoReceptors { get; set; }
        public virtual DbSet<Direccion> Direccions { get; set; }
        public virtual DbSet<Documento> Documentos { get; set; }
        public virtual DbSet<DocumentoAnalisi> DocumentoAnalises { get; set; }
        public virtual DbSet<DocumentoAnexo> DocumentoAnexos { get; set; }
        public virtual DbSet<DocumentoMuestra> DocumentoMuestras { get; set; }
        public virtual DbSet<Estado> Estados { get; set; }
        public virtual DbSet<Estatus> Estatuses { get; set; }
        public virtual DbSet<EstatusTipo> EstatusTipos { get; set; }
        public virtual DbSet<FrecuenciaMuestreo> FrecuenciaMuestreos { get; set; }
        public virtual DbSet<Laboratorio> Laboratorios { get; set; }
        public virtual DbSet<LaboratorioAcreditacion> LaboratorioAcreditacions { get; set; }
        public virtual DbSet<LaboratorioAprobacion> LaboratorioAprobacions { get; set; }
        public virtual DbSet<LaboratorioCliente> LaboratorioClientes { get; set; }
        public virtual DbSet<LaboratorioContacto> LaboratorioContactos { get; set; }
        public virtual DbSet<LaboratorioRepresentante> LaboratorioRepresentantes { get; set; }
        public virtual DbSet<LaboratorioSignatario> LaboratorioSignatarios { get; set; }
        public virtual DbSet<LaboratorioSucursal> LaboratorioSucursals { get; set; }
        public virtual DbSet<LaboratorioVigencium> LaboratorioVigencia { get; set; }
        public virtual DbSet<LaboratoriosCNA> LaboratoriosCNAs { get; set; }
        public virtual DbSet<Localidad> Localidads { get; set; }
        public virtual DbSet<Metodo> Metodos { get; set; }
        public virtual DbSet<Muestra> Muestras { get; set; }
        public virtual DbSet<MuestraValor> MuestraValors { get; set; }
        public virtual DbSet<Muestreador> Muestreadors { get; set; }
        public virtual DbSet<Municipio> Municipios { get; set; }
        public virtual DbSet<NivelConfiguracion> NivelConfiguracions { get; set; }
        public virtual DbSet<Norma> Normas { get; set; }
        public virtual DbSet<NormaConfiguracion> NormaConfiguracions { get; set; }
        public virtual DbSet<OperacionConsolidado> OperacionConsolidados { get; set; }
        public virtual DbSet<Operador> Operadors { get; set; }
        public virtual DbSet<OrdenTrabajo> OrdenTrabajos { get; set; }
        public virtual DbSet<Parametro> Parametros { get; set; }
        public virtual DbSet<ParametroMetodo> ParametroMetodos { get; set; }
        public virtual DbSet<Persona> Personas { get; set; }
        public virtual DbSet<SolicitudCancelacion> SolicitudCancelacions { get; set; }
        public virtual DbSet<SolicitudCancelacionDetalle> SolicitudCancelacionDetalles { get; set; }
        public virtual DbSet<TBitacora> TBitacoras { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<VW_BASE_GLOBAL_ANALISIS_SGAA> VW_BASE_GLOBAL_ANALISIS_SGAAs { get; set; }
        public virtual DbSet<Vw_AnalisisMensual> Vw_AnalisisMensuals { get; set; }
        public virtual DbSet<Vw_BusquedaAnalisi> Vw_BusquedaAnalises { get; set; }
        public virtual DbSet<Vw_BusquedaClienteAnalisi> Vw_BusquedaClienteAnalises { get; set; }
        public virtual DbSet<Vw_Conamer> Vw_Conamers { get; set; }
        public virtual DbSet<Vw_Habilitar_Anali> Vw_Habilitar_Analis { get; set; }
        public virtual DbSet<Vw_Ot_Grid> Vw_Ot_Grids { get; set; }
        public virtual DbSet<Vw_Reporte_Analis_ControlObligacione> Vw_Reporte_Analis_ControlObligaciones { get; set; }
        public virtual DbSet<Vw_Reporte_titulo> Vw_Reporte_titulos { get; set; }
        public virtual DbSet<vw_LaboratoriosPALA> vw_LaboratoriosPALAs { get; set; }
        public virtual DbSet<vw_reporte_SDT> vw_reporte_SDTs { get; set; }

        public virtual DbSet<tblProgramaCumplimientoNom001> tblProgramaCumplimientoNom001 { get; set; }

        public virtual DbSet<MensajePCNOM> MensajePCNOM { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=172.29.149.163\\MSSQLSERVER_FS;Database=Siralab_Produccion_Prueba;User Id= desarrollo_fs; Password=12345678;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Analisi>(entity =>
            {
                entity.HasKey(e => e.AnalisisID);

                entity.ToTable("Analisis");

                entity.HasIndex(e => new { e.OrdenTrabajoID, e.EstatusID, e.Firmada, e.Procesado }, "IDX_ANALISIS");

                entity.HasIndex(e => new { e.EstatusID, e.Firmada, e.Procesado }, "IDX_ANALISIS_FIRMADAS");

                entity.Property(e => e.AnalisisID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador del Análisis.");

                entity.Property(e => e.AnalisisPadreID).HasComment("Identificador del Análsis Padre.");

                entity.Property(e => e.AnexoID).HasComment("Indentificador que hace referencia al Anexo del Análisis.");

                entity.Property(e => e.Enviado).HasComment("Indica si el análisis envíado por el Laboratorio ha sido procesado o no.");

                entity.Property(e => e.FaltaPago).HasComment("Indica si el an�lisis no ha hecho el pago.");

                entity.Property(e => e.FechaFirma)
                    .HasColumnType("datetime")
                    .HasComment("Indica la fecha de la firma.");

                entity.Property(e => e.FechaProceso)
                    .HasColumnType("datetime")
                    .HasComment("Indica la fecha del proceso de análisis.");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Indica la fecha de registro del análisis.");

                entity.Property(e => e.Firmada).HasComment("Indica si el análisis ha sido firmado o no.");

                entity.Property(e => e.LaboratorioID).HasComment("Identificador del Laboratorio.");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("En éste campo es posible agregar un comentario u observaciones del proceso de toma de muestra. ");

                entity.Property(e => e.OrdenTrabajoID).HasComment("Identificador de la orden de trabajo.");

                entity.Property(e => e.Procesado).HasComment("Indica si el análisis envíado por el Laboratorio ha sido procesado o no.");

                entity.Property(e => e.NormaDQOCOT)
                   .HasMaxLength(3)
                   .IsUnicode(false)
                   .HasComment("Almacena el tipo de Norma seleccionado.");

                entity.HasOne(d => d.AnalisisPadre)
                    .WithMany(p => p.InverseAnalisisPadre)
                    .HasForeignKey(d => d.AnalisisPadreID)
                    .HasConstraintName("FK_Analisis_Analisis");

                entity.HasOne(d => d.Anexo)
                    .WithMany(p => p.Analisis)
                    .HasForeignKey(d => d.AnexoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Analisis_Anexo");

                entity.HasOne(d => d.Consecion)
                    .WithMany(p => p.Analisis)
                    .HasForeignKey(d => d.ConsecionID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Analisis_Consecion");

                entity.HasOne(d => d.Estatus)
                    .WithMany(p => p.Analisis)
                    .HasForeignKey(d => d.EstatusID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Analisis_Estatus");

                entity.HasOne(d => d.Laboratorio)
                    .WithMany(p => p.Analisis)
                    .HasForeignKey(d => d.LaboratorioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Analisis_Laboratorio");

                entity.HasOne(d => d.OrdenTrabajo)
                    .WithMany(p => p.Analisis)
                    .HasForeignKey(d => d.OrdenTrabajoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Analisis_OrdenTrabajo");
            });

            modelBuilder.Entity<AnalisisColaDetalle>(entity =>
            {
                entity.ToTable("AnalisisColaDetalle");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaFinalizacion).HasColumnType("datetime");
            });

            modelBuilder.Entity<AnalisisColaEncabezado>(entity =>
            {
                entity.ToTable("AnalisisColaEncabezado");

                entity.Property(e => e.FechaEnvio).HasColumnType("datetime");

                entity.Property(e => e.FechaFirma).HasColumnType("datetime");

                entity.HasOne(d => d.AnalisisColaDetalle)
                    .WithMany(p => p.AnalisisColaEncabezados)
                    .HasForeignKey(d => d.AnalisisColaDetalleID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AnalisisC__Anali__1D114BD1");

                entity.HasOne(d => d.Analisis)
                    .WithMany(p => p.AnalisisColaEncabezados)
                    .HasForeignKey(d => d.AnalisisID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AnalisisC__Anali__1C1D2798");
            });

            modelBuilder.Entity<AnalisisFaltaPago>(entity =>
            {
                entity.ToTable("AnalisisFaltaPago");

                entity.Property(e => e.Cadena).IsUnicode(false);

                entity.Property(e => e.Contenedor).IsUnicode(false);

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Firma)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.SelloDigital).IsUnicode(false);

                entity.HasOne(d => d.Analisis)
                    .WithMany(p => p.AnalisisFaltaPagos)
                    .HasForeignKey(d => d.AnalisisID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnalisisFaltaPago_Analisis");
            });

            modelBuilder.Entity<AnalisisFirma>(entity =>
            {
                entity.ToTable("AnalisisFirma");

                entity.Property(e => e.AnalisisFirmaID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador de la tabla de la Firma de  los Análisis.");

                entity.Property(e => e.AnalisisID).HasComment("Identificador del Análisis.");

                entity.Property(e => e.Cadena)
                    .IsUnicode(false)
                    .HasComment("Almacena la cadena de la firma.");

                entity.Property(e => e.Contenedor).IsUnicode(false);

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Indica la fecha del registro.");

                entity.Property(e => e.Firma)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasComment("Almacena la Firma del Laboratorio.");

                entity.Property(e => e.RFC)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasComment("Almacena el RFC.");

                entity.Property(e => e.SelloDigital).IsUnicode(false);

                entity.HasOne(d => d.Analisis)
                    .WithMany(p => p.AnalisisFirmas)
                    .HasForeignKey(d => d.AnalisisID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnalisisFirma_Analisis");
            });

            modelBuilder.Entity<Anexo>(entity =>
            {
                entity.ToTable("Anexo");

                entity.Property(e => e.AnexoID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador del  Anexo.");

                entity.Property(e => e.Anexo1)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Anexo")
                    .HasComment("Indica el número de Anexo.");

                entity.Property(e => e.ConsecionID).HasComment("Identificador del punto de descarga.");

                entity.Property(e => e.CuerpoReceptorID).HasComment("Identificador del cuerpo receptor.");

                entity.Property(e => e.Datum)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Indica un punto de referencia del lugar en donde se ha tomado la muestra.");

                entity.Property(e => e.DireccionID).HasComment("Muestra el identificador de la dirección del punto de descarga.");

                entity.Property(e => e.Efluente).HasComment("Indica si las aguas son efluentes o no.");

                entity.Property(e => e.FuenteAbastecimiento)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasComment("Indica la fuente de abastecimiento.");

                entity.Property(e => e.Latitud)
                    .HasColumnType("decimal(18, 14)")
                    .HasComment("Indica la localización del lugar de donde fue tomada la muestra.");

                entity.Property(e => e.Longitud)
                    .HasColumnType("decimal(18, 14)")
                    .HasComment("Indica la longitud del lugar en donde fue tomada la muestra.");

                entity.Property(e => e.MarcaMedidor)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Medidor)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número del medidor");

                entity.Property(e => e.NombreCuerpoReceptor)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasComment("Indica el nombre del cuerpo receptor.");

                entity.Property(e => e.OrigenDescarga)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasComment("Indica el origen de la descarga.");

                entity.Property(e => e.UsoAguaID).HasComment("Identificador del Tipo de Uso del Agua.");

                entity.HasOne(d => d.Consecion)
                    .WithMany(p => p.Anexos)
                    .HasForeignKey(d => d.ConsecionID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Anexo_Consecion");

                entity.HasOne(d => d.CuerpoReceptor)
                    .WithMany(p => p.Anexos)
                    .HasForeignKey(d => d.CuerpoReceptorID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Anexo_CuerpoReceptor");

                entity.HasOne(d => d.Direccion)
                    .WithMany(p => p.Anexos)
                    .HasForeignKey(d => d.DireccionID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Anexo_Direccion");

                entity.HasOne(d => d.UsoAgua)
                    .WithMany(p => p.Anexos)
                    .HasForeignKey(d => d.UsoAguaID)
                    .HasConstraintName("FK_Anexo_CatUsoAgua");
            });

            modelBuilder.Entity<AnexoMedidor>(entity =>
            {
                entity.ToTable("AnexoMedidor");

                entity.Property(e => e.AnexoMedidorID).ValueGeneratedNever();

                entity.Property(e => e.MARCA_MEDIDOR)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.numeroMedidor)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Anexo)
                    .WithMany(p => p.AnexoMedidors)
                    .HasForeignKey(d => d.AnexoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnexoMedidor_Anexo");
            });

            modelBuilder.Entity<CatDiaInhabil>(entity =>
            {
                entity.HasKey(e => e.DiaInhabilID);

                entity.ToTable("CatDiaInhabil");

                entity.Property(e => e.DiaInhabil).HasColumnType("datetime");
            });

            modelBuilder.Entity<CatGrupoMetodo>(entity =>
            {
                entity.HasKey(e => e.IdGrupoMetodo);

                entity.ToTable("CatGrupoMetodo");

                entity.Property(e => e.IdGrupoMetodo).HasComment("Identificador del grupo del m�todo.");

                entity.Property(e => e.Activo).HasComment("Estado del grupo del m�todo.");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Descripci�n del grupo del m�todo.");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasComment("Nombre del grupo del m�todo.");
            });

            modelBuilder.Entity<CatModulo>(entity =>
            {
                entity.HasKey(e => e.ModuloID);

                entity.Property(e => e.ModuloID).ValueGeneratedNever();

                entity.Property(e => e.CssClass)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.URL)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ModuloPadre)
                    .WithMany(p => p.InverseModuloPadre)
                    .HasForeignKey(d => d.ModuloPadreID)
                    .HasConstraintName("FK_CatModulos_CatModulos");
            });

            modelBuilder.Entity<CatPagina>(entity =>
            {
                entity.HasKey(e => e.PaginaID);

                entity.ToTable("CatPagina");

                entity.Property(e => e.PaginaID).ValueGeneratedNever();

                entity.Property(e => e.CssClass)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Indica la fecha en que se registró el rol de usuario.");

                entity.Property(e => e.URL)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Modulo)
                    .WithMany(p => p.CatPaginas)
                    .HasForeignKey(d => d.ModuloID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatPagina_CatModulos");

                entity.HasOne(d => d.Sistema)
                    .WithMany(p => p.CatPaginas)
                    .HasForeignKey(d => d.SistemaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatPagina_CatSistema");
            });

            modelBuilder.Entity<CatPermisosRol>(entity =>
            {
                entity.HasKey(e => e.PermisosRolID);

                entity.ToTable("CatPermisosRol");

                entity.Property(e => e.PermisosRolID).ValueGeneratedNever();

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Indica la fecha en que se registó el usuario.");

                entity.HasOne(d => d.Pagina)
                    .WithMany(p => p.CatPermisosRols)
                    .HasForeignKey(d => d.PaginaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatPermisosRol_CatPagina");

                entity.HasOne(d => d.UsuarioRol)
                    .WithMany(p => p.CatPermisosRols)
                    .HasForeignKey(d => d.UsuarioRolID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatPermisosRol_CatUsuarioRol");
            });

            modelBuilder.Entity<CatSistema>(entity =>
            {
                entity.HasKey(e => e.SistemaID);

                entity.ToTable("CatSistema");

                entity.Property(e => e.SistemaID).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatTipoContacto>(entity =>
            {
                entity.HasKey(e => e.TipoContactoID);

                entity.ToTable("CatTipoContacto");

                entity.Property(e => e.TipoContactoID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador único del tipo de contacto.");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Nombre del tipo de contacto.");

                entity.Property(e => e.RequiereInfo).HasComment("Indica si la entidad tipo contacto requiere información.");
            });

            modelBuilder.Entity<CatTipoCuerpoReceptor>(entity =>
            {
                entity.HasKey(e => e.TipoCuerpoReceptorID);

                entity.ToTable("CatTipoCuerpoReceptor");

                entity.Property(e => e.TipoCuerpoReceptorID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador del Tipo de Cuerpo Receptor.");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Nombre del Cuerpo Receptor.");
            });

            modelBuilder.Entity<CatTipoDocumentoAnexo>(entity =>
            {
                entity.HasKey(e => e.TipoDocumentoAnexoID);

                entity.ToTable("CatTipoDocumentoAnexo");

                entity.Property(e => e.TipoDocumentoAnexoID).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatTipoDocumentoMuestra>(entity =>
            {
                entity.HasKey(e => e.TipoDocumentoMuestraID);

                entity.ToTable("CatTipoDocumentoMuestra");

                entity.Property(e => e.TipoDocumentoMuestraID).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatTipoSignatario>(entity =>
            {
                entity.HasKey(e => e.TipoSignatarioID);

                entity.ToTable("CatTipoSignatario");

                entity.Property(e => e.TipoSignatarioID).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatUnidadMedidum>(entity =>
            {
                entity.HasKey(e => e.UnidadMedidaID);

                entity.Property(e => e.UnidadMedidaID).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatUsoAgua>(entity =>
            {
                entity.HasKey(e => e.UsoAguaID);

                entity.ToTable("CatUsoAgua");

                entity.Property(e => e.UsoAguaID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador del Tipo de Uso del Agua.");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Nombre del Tipo del Uso del Agua.");
            });

            modelBuilder.Entity<CatUsuarioRol>(entity =>
            {
                entity.HasKey(e => e.UsuarioRolID);

                entity.ToTable("CatUsuarioRol");

                entity.Property(e => e.UsuarioRolID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador de la tabla de roles de usuario.");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si el rol  de usuario está Activo o Inactivo.");

                entity.Property(e => e.Administrable)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si el rol es administrador o no.");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Indica la fecha en que se registró el rol de usuario.");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("Nombre del rol de usuario.");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Cliente");

                entity.HasIndex(e => new { e.Nombre, e.ApellidoPaterno, e.ApellidoMaterno, e.RFC }, "IDX_ClienteRow");

                entity.Property(e => e.ClienteID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador del Cliente.");

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasComment("Indica el apellido materno del cliente.");

                entity.Property(e => e.ApellidoPaterno)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasComment("Indica el apellido parterno del cliente.");

                entity.Property(e => e.CURP)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Hace referencia al CURP del cliente.");

                entity.Property(e => e.Correo)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DireccionID).HasComment("Identificador que hace referencia a la dirección.");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Indica el nombre del cliente.");

                entity.Property(e => e.RFC)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasComment("Hace referencia al Registro Federal de Contribuyentes (RFC) del cliente.");

                entity.HasOne(d => d.Direccion)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.DireccionID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cliente_Direccion");
            });

            modelBuilder.Entity<ClienteContacto>(entity =>
            {
                entity.ToTable("ClienteContacto");

                entity.Property(e => e.ClienteContactoID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador de Cliente-Contacto.");

                entity.Property(e => e.ClienteID).HasComment("Indentificador único del Cliente.");

                entity.Property(e => e.PersonaID).HasComment("Identificador único de la persona.");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.ClienteContactos)
                    .HasForeignKey(d => d.ClienteID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClienteContacto_Cliente");

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.ClienteContactos)
                    .HasForeignKey(d => d.PersonaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClienteContacto_Persona");
            });

            modelBuilder.Entity<ColoniaTipo>(entity =>
            {
                entity.ToTable("ColoniaTipo");

                entity.Property(e => e.ColoniaTipoID).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Colonium>(entity =>
            {
                entity.HasKey(e => e.ColoniaID);

                entity.Property(e => e.ColoniaID).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.ColoniaTipo)
                    .WithMany(p => p.Colonia)
                    .HasForeignKey(d => d.ColoniaTipoID)
                    .HasConstraintName("FK_Colonia_ColoniaTipo");

                entity.HasOne(d => d.Municipio)
                    .WithMany(p => p.Colonia)
                    .HasForeignKey(d => new { d.MunicipioID, d.EstadoID })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Colonia_Municipio");
            });

            modelBuilder.Entity<ConfiguracionGrupoMetodo>(entity =>
            {
                entity.HasKey(e => new { e.IdGrupoMetodo, e.IdMetodo });

                entity.ToTable("ConfiguracionGrupoMetodo");

                entity.Property(e => e.IdGrupoMetodo).HasComment("Identificador del grupo del m�todo.");

                entity.Property(e => e.IdMetodo).HasComment("Identificador del m�todo.");

                entity.Property(e => e.Activo).HasComment("Estado de la configuraci�n Grupo - Metodo.");

                entity.HasOne(d => d.IdGrupoMetodoNavigation)
                    .WithMany(p => p.ConfiguracionGrupoMetodos)
                    .HasForeignKey(d => d.IdGrupoMetodo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConfiguracionGrupoMetodo_CatGrupoMetodo");

                entity.HasOne(d => d.IdMetodoNavigation)
                    .WithMany(p => p.ConfiguracionGrupoMetodos)
                    .HasForeignKey(d => d.IdMetodo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConfiguracionGrupoMetodo_Metodo");
            });

            modelBuilder.Entity<Consecion>(entity =>
            {
                entity.ToTable("Consecion");

                entity.Property(e => e.ConsecionID).ValueGeneratedNever();

                entity.Property(e => e.ClienteID).HasComment("Muestra el identificador único asociado a un cliente.");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasComment("Muestra la descripción del punto de descarga.");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Consecions)
                    .HasForeignKey(d => d.ClienteID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Consecion_Cliente");
            });

            modelBuilder.Entity<Contacto>(entity =>
            {
                entity.ToTable("Contacto");

                entity.Property(e => e.ContactoID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador único del contacto.");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si el contacto esta Activo o Inactivo.");

                entity.Property(e => e.Contacto1)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("Contacto")
                    .HasComment("Nombre del contacto");

                entity.Property(e => e.Extension)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Indica la extensión del contacto.");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Muestra la fecha en que se realizó el registro del contacto.");

                entity.Property(e => e.PersonaID).HasComment("Indentificador de la persona.");

                entity.Property(e => e.TipoContactoID).HasComment("Señala con un identificador el tipo de contacto al que se está haciendo referencia.");

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.Contactos)
                    .HasForeignKey(d => d.PersonaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contacto_Persona");

                entity.HasOne(d => d.TipoContacto)
                    .WithMany(p => p.Contactos)
                    .HasForeignKey(d => d.TipoContactoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contacto_CatTipoContacto");
            });

            modelBuilder.Entity<Correspondencia_CR_UA_SIRFIC_SIRALAB>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Correspondencia_CR_UA_SIRFIC_SIRALAB");
            });

            modelBuilder.Entity<CuerpoReceptor>(entity =>
            {
                entity.ToTable("CuerpoReceptor");

                entity.Property(e => e.CuerpoReceptorID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador del Cuerpo Receptor.");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Nombre del Cuerpo Receptor.");

                entity.Property(e => e.TipoCuerpoReceptorID).HasComment("Identificador del Tipo de Cuerpo Receptor.");

                entity.HasOne(d => d.TipoCuerpoReceptor)
                    .WithMany(p => p.CuerpoReceptors)
                    .HasForeignKey(d => d.TipoCuerpoReceptorID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CuerpoReceptor_CatTipoCuerpoReceptor");
            });

            modelBuilder.Entity<Direccion>(entity =>
            {
                entity.ToTable("Direccion");

                entity.Property(e => e.DireccionID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador de la tabla de Dirección.");

                entity.Property(e => e.Calle)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasComment("Nombre de la Calle.");

                entity.Property(e => e.ColoniaID).HasComment("Identificador de la Colonia.");

                entity.Property(e => e.EstadoID).HasComment("Identificador del Estado.");

                entity.Property(e => e.LocalidadID).HasComment("Identificador de la Localidad.");

                entity.Property(e => e.MunicipioID).HasComment("Identificador del Municipio.");

                entity.Property(e => e.NumeroExterior)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Número Exterior del domicilio.");

                entity.Property(e => e.NumeroInterior)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Número Interior del domicilio.");

                entity.HasOne(d => d.Colonia)
                    .WithMany(p => p.Direccions)
                    .HasForeignKey(d => d.ColoniaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Direccion_Colonia");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Direccions)
                    .HasForeignKey(d => d.EstadoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Direccion_Estado");

                entity.HasOne(d => d.Municipio)
                    .WithMany(p => p.Direccions)
                    .HasForeignKey(d => new { d.MunicipioID, d.EstadoID })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Direccion_Municipio");

                entity.HasOne(d => d.Localidad)
                    .WithMany(p => p.Direccions)
                    .HasForeignKey(d => new { d.LocalidadID, d.MunicipioID, d.EstadoID })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Direccion_Localidad");
            });

            modelBuilder.Entity<Documento>(entity =>
            {
                entity.ToTable("Documento");

                entity.Property(e => e.DocumentoID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador del Documento.");

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Indica la Extensión del documento.");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Hace referencia a la Fecha de Registro del Documento.");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Específica el nombre del documento.");

                entity.Property(e => e.Ruta)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Indica la ruta del documento.");

                entity.Property(e => e.TipoMime)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("Hace referencia al Tipo MIME (extensiones multipropósito de correo de internet).");
            });

            modelBuilder.Entity<DocumentoAnalisi>(entity =>
            {
                entity.HasKey(e => e.DocumentoAnalisisID);

                entity.ToTable("DocumentoAnalisis");

                entity.Property(e => e.DocumentoAnalisisID).ValueGeneratedNever();

                entity.HasOne(d => d.Analisis)
                    .WithMany(p => p.DocumentoAnalisis)
                    .HasForeignKey(d => d.AnalisisID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentoAnalisis_Analisis");

                entity.HasOne(d => d.Documento)
                    .WithMany(p => p.DocumentoAnalisis)
                    .HasForeignKey(d => d.DocumentoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentoAnalisis_Documento");
            });

            modelBuilder.Entity<DocumentoAnexo>(entity =>
            {
                entity.ToTable("DocumentoAnexo");

                entity.Property(e => e.DocumentoAnexoID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador del Documento Anexo.");

                entity.Property(e => e.DocumentoID).HasComment("Identificador del Documento.");

                entity.Property(e => e.OrdenTrabajoID).HasComment("Identificador del Anexo.");

                entity.Property(e => e.TipoDocumentoAnexoID).HasComment("Específica el Tipo de documento a través de su identificador.");

                entity.HasOne(d => d.Documento)
                    .WithMany(p => p.DocumentoAnexos)
                    .HasForeignKey(d => d.DocumentoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentoAnexo_Documento");

                entity.HasOne(d => d.OrdenTrabajo)
                    .WithMany(p => p.DocumentoAnexos)
                    .HasForeignKey(d => d.OrdenTrabajoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentoAnexo_OrdenTrabajo");

                entity.HasOne(d => d.TipoDocumentoAnexo)
                    .WithMany(p => p.DocumentoAnexos)
                    .HasForeignKey(d => d.TipoDocumentoAnexoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentoAnexo_CatTipoDocumentoAnexo");
            });

            modelBuilder.Entity<DocumentoMuestra>(entity =>
            {
                entity.ToTable("DocumentoMuestra");

                entity.Property(e => e.DocumentoMuestraID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador de Documento - Muestra.");

                entity.Property(e => e.DocumentoID).HasComment("Identificador del documento.");

                entity.Property(e => e.MuestraID).HasComment("Identificador de la Muestra.");

                entity.Property(e => e.TipoDocumentoMuestraID).HasComment("Identificador que señala el tipo de documento.");

                entity.HasOne(d => d.Documento)
                    .WithMany(p => p.DocumentoMuestras)
                    .HasForeignKey(d => d.DocumentoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentoMuestra_Documento");

                entity.HasOne(d => d.Muestra)
                    .WithMany(p => p.DocumentoMuestras)
                    .HasForeignKey(d => d.MuestraID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentoMuestra_Muestra");

                entity.HasOne(d => d.TipoDocumentoMuestra)
                    .WithMany(p => p.DocumentoMuestras)
                    .HasForeignKey(d => d.TipoDocumentoMuestraID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentoMuestra_CatTipoDocumentoMuestra");
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.ToTable("Estado");

                entity.Property(e => e.EstadoID).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Estatus>(entity =>
            {
                entity.ToTable("Estatus");

                entity.Property(e => e.EstatusID).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.HasOne(d => d.EstatusTipo)
                    .WithMany(p => p.Estatuses)
                    .HasForeignKey(d => d.EstatusTipoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Estatus_EstatusTipo");
            });

            modelBuilder.Entity<EstatusTipo>(entity =>
            {
                entity.ToTable("EstatusTipo");

                entity.Property(e => e.EstatusTipoID).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FrecuenciaMuestreo>(entity =>
            {
                entity.ToTable("FrecuenciaMuestreo");

                entity.Property(e => e.FrecuenciaMuestreoID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador de la tabla de Frecuencia de Muestreo.");

                entity.Property(e => e.HorasMaximo).HasComment("Indica el intervalo máximo de tiempo, medido en horas, entre la toma de muestras simples.");

                entity.Property(e => e.HorasMinimo).HasComment("Indica el intervalo mínimo de tiempo, medido en horas, entre la toma de muestras simples.");

                entity.Property(e => e.MuestrasSimples).HasComment("Indica cuántas muestras simples han sido tomadas.");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Indica las horas por día que opera el proceso generador de la descarga.");

                entity.Property(e => e.OperadorIDMaximo).HasComment("Indica el número máximo de muestras que pueden ser tomadas.");

                entity.Property(e => e.OperadorIDMinimo).HasComment("Indica el número mínimo de muestras que deben ser tomadas.");

                entity.HasOne(d => d.OperadorIDMaximoNavigation)
                    .WithMany(p => p.FrecuenciaMuestreoOperadorIDMaximoNavigations)
                    .HasForeignKey(d => d.OperadorIDMaximo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FrecuenciaMuestreo_Operador1");

                entity.HasOne(d => d.OperadorIDMinimoNavigation)
                    .WithMany(p => p.FrecuenciaMuestreoOperadorIDMinimoNavigations)
                    .HasForeignKey(d => d.OperadorIDMinimo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FrecuenciaMuestreo_Operador");
            });

            modelBuilder.Entity<Laboratorio>(entity =>
            {
                entity.ToTable("Laboratorio");

                entity.Property(e => e.LaboratorioID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador de la tabla de Laboratorios.");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica el estatus del Laboratorio, si está Activo o no.");

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasComment("Indica el Apellido Materno del Laboratorio (Sólo en caso de ser persona física).");

                entity.Property(e => e.ApellidoPaterno)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasComment("Indica el Apellido Paterno del Laboratorio (Sólo en caso de ser persona física).");

                entity.Property(e => e.CURP)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Hace referencia al CURP (Sólo en caso de ser persona física).");

                entity.Property(e => e.DireccionID).HasComment("Identificador de la dirección del laboratorio.");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Hace referencia a la Fecha de Registro del Laboratorio.");

                entity.Property(e => e.Fisica).HasComment("Indica si el laboratorio se registra como persona física o no.");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Hace referencia al Nombre del Laboratorio.");

                entity.Property(e => e.RFC)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasComment("Hace referencia al Registro Federal de Contribuyentes (RFC) del Laboratorio.");

                entity.HasOne(d => d.Direccion)
                    .WithMany(p => p.Laboratorios)
                    .HasForeignKey(d => d.DireccionID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Laboratorio_Direccion");
            });

            modelBuilder.Entity<LaboratorioAcreditacion>(entity =>
            {
                entity.ToTable("LaboratorioAcreditacion");

                entity.Property(e => e.LaboratorioAcreditacionID)
                    .ValueGeneratedNever()
                    .HasComment("Indica el identificador de la Acreditación del Laboratorio.");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si la Acreditación del Laboratorio está activa o no.");

                entity.Property(e => e.FechaFin)
                    .HasColumnType("date")
                    .HasComment("Señala la vigencia de la acreditación del Laboratorio.");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("date")
                    .HasComment("Indica la Fecha de Inicio de la Acreditación del Laboratorio por parte de la EMA.");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Hace referencia a la fecha de registro de la Acreditación del Laboratorio.");

                entity.Property(e => e.LaboratorioID).HasComment("Indica el identificador del Laboratorio.");

                entity.Property(e => e.Numero)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("Hace referencia al número de acreditación del Laboratorio proporcionado por la Entidad Méxicana de Acreditación (EMA).");

                entity.Property(e => e.Token)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Laboratorio)
                    .WithMany(p => p.LaboratorioAcreditacions)
                    .HasForeignKey(d => d.LaboratorioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LaboratorioAcreditacion_Laboratorio");
            });

            modelBuilder.Entity<LaboratorioAprobacion>(entity =>
            {
                entity.ToTable("LaboratorioAprobacion");

                entity.Property(e => e.LaboratorioAprobacionID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador de la aprobación del Laboratorio.");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si la Aprobación del Laboratorio está activa o no.");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Señala la fecha de registro de la aprobación del Laboratorio.");

                entity.Property(e => e.LaboratorioID).HasComment("Identificador del Laboratorio.");

                entity.Property(e => e.LaboratorioVigenciaID).HasComment("Identificador de la Vigencia de la aprobación del Laboratorio.");

                entity.Property(e => e.ParametroMetodoID).HasComment("Identificador de la relación entre Parámetro-Método.");

                entity.HasOne(d => d.Laboratorio)
                    .WithMany(p => p.LaboratorioAprobacions)
                    .HasForeignKey(d => d.LaboratorioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LaboratorioAprobacion_Laboratorio");

                entity.HasOne(d => d.LaboratorioVigencia)
                    .WithMany(p => p.LaboratorioAprobacions)
                    .HasForeignKey(d => d.LaboratorioVigenciaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LaboratorioAprobacion_LaboratorioVigencia");

                entity.HasOne(d => d.ParametroMetodo)
                    .WithMany(p => p.LaboratorioAprobacions)
                    .HasForeignKey(d => d.ParametroMetodoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LaboratorioAprobacion_ParametroMetodo");
            });

            modelBuilder.Entity<LaboratorioCliente>(entity =>
            {
                entity.ToTable("LaboratorioCliente");

                entity.Property(e => e.LaboratorioClienteID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador de Laboratorio-Cliente.");

                entity.Property(e => e.ClienteID).HasComment("Identificador de cliente.");

                entity.Property(e => e.LaboratorioID).HasComment("Identificador único de un Laboratorio.");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.LaboratorioClientes)
                    .HasForeignKey(d => d.ClienteID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LaboratorioCliente_Cliente");

                entity.HasOne(d => d.Laboratorio)
                    .WithMany(p => p.LaboratorioClientes)
                    .HasForeignKey(d => d.LaboratorioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LaboratorioCliente_Laboratorio");
            });

            modelBuilder.Entity<LaboratorioContacto>(entity =>
            {
                entity.ToTable("LaboratorioContacto");

                entity.Property(e => e.LaboratorioContactoID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador del Contacto del Laboratorio.");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Identificador s� esta activo.");

                entity.Property(e => e.FechaEliminacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de eliminaci�n.");

                entity.Property(e => e.LaboratorioID).HasComment("Identificador del Laboratorio.");

                entity.Property(e => e.PersonaID).HasComment("Identificador de la persona.");

                entity.HasOne(d => d.Laboratorio)
                    .WithMany(p => p.LaboratorioContactos)
                    .HasForeignKey(d => d.LaboratorioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LaboratorioContacto_Laboratorio");

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.LaboratorioContactos)
                    .HasForeignKey(d => d.PersonaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LaboratorioContacto_Persona");
            });

            modelBuilder.Entity<LaboratorioRepresentante>(entity =>
            {
                entity.ToTable("LaboratorioRepresentante");

                entity.Property(e => e.LaboratorioRepresentanteID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador del representante del laboratorio.");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica el estatus del representante del laboratorio, señala si está Activo o no.");

                entity.Property(e => e.EsContacto).HasComment("Es Laboratorio Contacto.");

                entity.Property(e => e.FechaEliminacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de eliminaci�n.");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Indica la Fecha de Registro del Representante del Laboratorio.");

                entity.Property(e => e.LaboratorioID).HasComment("Indentificador del laboratorio.");

                entity.Property(e => e.PersonaID).HasComment("Identificador de la persona.");

                entity.Property(e => e.RFC)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasComment("Indica el Registro Federal de Contribuyentes del Representante Fiscal del Laboratorio.");

                entity.HasOne(d => d.Laboratorio)
                    .WithMany(p => p.LaboratorioRepresentantes)
                    .HasForeignKey(d => d.LaboratorioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LaboratorioRepresentante_Laboratorio");

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.LaboratorioRepresentantes)
                    .HasForeignKey(d => d.PersonaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LaboratorioRepresentante_Persona");
            });

            modelBuilder.Entity<LaboratorioSignatario>(entity =>
            {
                entity.ToTable("LaboratorioSignatario");

                entity.Property(e => e.LaboratorioSignatarioID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador de la tabla de los Signatarios del Laboratorios.");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si el Signatario del Laboratorio está activo o no.");

                entity.Property(e => e.FechaEliminacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de eliminaci�n.");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Indica la fecha de registro del signatario del  Laboratorio.");

                entity.Property(e => e.LaboratorioID).HasComment("Indica el identificador del Laboratorio.");

                entity.Property(e => e.PersonaID).HasComment("Está compuesto por el identificador de la persona.");

                entity.Property(e => e.TipoSignatarioID).HasComment("Está compuesto por el identificador del tipo de signatario.");

                entity.HasOne(d => d.Laboratorio)
                    .WithMany(p => p.LaboratorioSignatarios)
                    .HasForeignKey(d => d.LaboratorioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LaboratorioSignatario_Laboratorio");

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.LaboratorioSignatarios)
                    .HasForeignKey(d => d.PersonaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LaboratorioSignatario_Persona");

                entity.HasOne(d => d.TipoSignatario)
                    .WithMany(p => p.LaboratorioSignatarios)
                    .HasForeignKey(d => d.TipoSignatarioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LaboratorioSignatario_CatTipoSignatario");
            });

            modelBuilder.Entity<LaboratorioSucursal>(entity =>
            {
                entity.ToTable("LaboratorioSucursal");

                entity.Property(e => e.LaboratorioSucursalID).ValueGeneratedNever();

                entity.HasOne(d => d.LaboratorioHijo)
                    .WithMany(p => p.LaboratorioSucursalLaboratorioHijos)
                    .HasForeignKey(d => d.LaboratorioHijoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LaboratorioSucursal_Laboratorio1");

                entity.HasOne(d => d.LaboratorioPadre)
                    .WithMany(p => p.LaboratorioSucursalLaboratorioPadres)
                    .HasForeignKey(d => d.LaboratorioPadreID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LaboratorioSucursal_Laboratorio");
            });

            modelBuilder.Entity<LaboratorioVigencium>(entity =>
            {
                entity.HasKey(e => e.LaboratorioVigenciaID);

                entity.Property(e => e.LaboratorioVigenciaID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador de la Vigencia del Laboratorio.");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si el Laboratorio está activo o no.");

                entity.Property(e => e.FechaFin)
                    .HasColumnType("date")
                    .HasComment("Indica la fecha final de la vigencia de la acreditación del Laboratorio.");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("date")
                    .HasComment("Indica la fecha inicial de la vigencia de la acreditación del Laboratorio.");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Indica la fecha de registro del Laboratorio.");

                entity.Property(e => e.LaboratorioID).HasComment("Identificador de la Acreditación del Laboratorio por parte de la EMA.");

                entity.Property(e => e.Numero)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("Número de Acreditación del Laboratorio por parte de la Entidad Mexicana de Acreditación (EMA).");

                entity.HasOne(d => d.Laboratorio)
                    .WithMany(p => p.LaboratorioVigencia)
                    .HasForeignKey(d => d.LaboratorioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LaboratorioVigencia_Laboratorio");
            });

            modelBuilder.Entity<LaboratoriosCNA>(entity =>
            {
                entity.ToTable("LaboratoriosCNA");

                entity.Property(e => e.FechaFin).HasColumnType("datetime");

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.RfcLaboratorio)
                    .HasMaxLength(13)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Localidad>(entity =>
            {
                entity.HasKey(e => new { e.LocalidadID, e.MunicipioID, e.EstadoID });

                entity.ToTable("Localidad");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Municipio)
                    .WithMany(p => p.Localidads)
                    .HasForeignKey(d => new { d.MunicipioID, d.EstadoID })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Localidad_Municipio");
            });

            modelBuilder.Entity<Metodo>(entity =>
            {
                entity.ToTable("Metodo");

                entity.Property(e => e.MetodoID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador del Método.");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si el método esta en estatus Activo o no.");

                entity.Property(e => e.Descripcion)
                    .IsUnicode(false)
                    .HasComment("Éste campo permite agregar una descripción breve del Método.");

                entity.Property(e => e.FechaFinal)
                    .HasColumnType("datetime")
                    .HasComment("Hace referencia a fecha final de operación del método.");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("date")
                    .HasComment("Hace referencia a la Fecha de Inicio de Operación del Método.");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Indica la Fecha de Registro del Método.");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasComment("Indica el nombre del método.");
            });

            modelBuilder.Entity<Muestra>(entity =>
            {
                entity.ToTable("Muestra");

                entity.HasIndex(e => e.NivelID, "IDX_MUESTRA");

                entity.HasIndex(e => e.NivelID, "IDX_MUESTRA_ANL_MUESTRA");

                entity.HasIndex(e => new { e.AnalisisID, e.NivelID }, "IDX_MUESTRA_FECHA");

                entity.Property(e => e.MuestraID)
                    .ValueGeneratedNever()
                    .HasComment("Es el identificador de la muestra.");

                entity.Property(e => e.AnalisisID).HasComment("Representa el identificador del an�lisis de resultados de laboratorio.");

                entity.Property(e => e.Caudal)
                    .HasColumnType("decimal(18, 9)")
                    .HasComment("Indica la cantidad de fluido que pasa en una unidad de tiempo.");

                entity.Property(e => e.Datum)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Indica un punto de referencia en la superficie terrestre.");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Est� compuesto por una descripci�n de la muestra.");

                entity.Property(e => e.DireccionID).HasComment("Indica el identificador de la direcci�n de la muestra.");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasComment("Hace referencia a la fecha en que fue tomada la muestra por parte del Laboratorio.");

                entity.Property(e => e.FechaConformacion).HasColumnType("datetime");

                entity.Property(e => e.FechaRecepcion)
                    .HasColumnType("datetime")
                    .HasComment("Se registra la fecha en que la muestra fue recibida en el sistema.");

                entity.Property(e => e.Identificacion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Hace referencia a la identificaci�n de la muestra.");

                entity.Property(e => e.Latitud)
                    .HasColumnType("decimal(18, 14)")
                    .HasComment("Almacena un punto determinado en la Tierra.");

                entity.Property(e => e.Longitud)
                    .HasColumnType("decimal(18, 14)")
                    .HasComment("Almacena una medida de una dimensi�n.");

                entity.Property(e => e.NivelID).HasComment("Indica el nivel de la muestra (Simple, Compuesta o Mensual).");

                entity.Property(e => e.Numero)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("Indica el n�mero de muestra.");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("En �ste campo es posible agregar un comentario u observaciones del proceso de toma de muestra. ");

                entity.Property(e => e.ValHojaCampo).HasColumnType("decimal(1, 0)");

                entity.Property(e => e.ValMuestraC).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Analisis)
                    .WithMany(p => p.Muestras)
                    .HasForeignKey(d => d.AnalisisID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Muestra_Analisis");

                entity.HasOne(d => d.Direccion)
                    .WithMany(p => p.Muestras)
                    .HasForeignKey(d => d.DireccionID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Muestra_Direccion");

                entity.HasOne(d => d.Nivel)
                    .WithMany(p => p.Muestras)
                    .HasForeignKey(d => d.NivelID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Muestra_NivelConfiguracion");
            });

            modelBuilder.Entity<MuestraValor>(entity =>
            {
                entity.ToTable("MuestraValor");

                entity.HasIndex(e => e.MuestraID, "IDX_MUESTRAVALOR");

                entity.Property(e => e.MuestraValorID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador del valor de la muestra.");

                entity.Property(e => e.AceptaValor)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si la configuración de la norma acepta valor o no.");

                entity.Property(e => e.Exenta).HasComment("Indica si el parámetro es obligatorio para la norma (false - es obligatorio, true - no es obligatorio)");

                entity.Property(e => e.Exento).HasComment("Indica si el parámetro queda exento de análisis.");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Indica la Fecha de Registro del valor de la muestra.");

                entity.Property(e => e.GeneradoSP).HasComment("Indica si el parámetro fue generado por SP");

                entity.Property(e => e.Grupo).HasComment("Indica si el Parámetro capturado es un grupo calculado");

                entity.Property(e => e.LCMC).HasDefaultValueSql("((0))");

                entity.Property(e => e.LC_MC).HasColumnType("decimal(21, 9)");

                entity.Property(e => e.LaboratorioID).HasComment("Identificador del Laboratorio.");

                entity.Property(e => e.LimiteInferior)
                    .HasColumnType("decimal(21, 9)")
                    .HasComment("Hace referencia al límite inferior permisible de la norma.");

                entity.Property(e => e.LimiteInferiorLFD)
                    .HasColumnType("decimal(21, 9)")
                    .HasComment("Hace referencia al límite inferior permisible de la norma.");

                entity.Property(e => e.LimiteSuperior)
                    .HasColumnType("decimal(21, 9)")
                    .HasComment("Hace referencia al límite superior permisible de la norma.");

                entity.Property(e => e.LimiteSuperiorLFD)
                    .HasColumnType("decimal(21, 9)")
                    .HasComment("Hace referencia al límite superior permisible de la norma.");

                //entity.Property(e => e.MaxAceptado).HasColumnType("decimal(18, 9)");

                //entity.Property(e => e.MinAceptado).HasColumnType("decimal(18, 9)");

                entity.Property(e => e.MuestraID).HasComment("Identificador de la muestra.");

                entity.Property(e => e.NoAnalizado).HasComment("Indica si el parámetro ha sido analizado por el laboratorio a petición del cliente.");

                entity.Property(e => e.OperacionID).HasComment("Indica sí el parámetro es calculado de su antecesor.");

                entity.Property(e => e.ParametroID).HasComment("Identificador del  Parámetro.");

                entity.Property(e => e.ParametroMetodoID).HasComment("Indica el parámetro de la muestra.");

                entity.Property(e => e.Valor)
                    .HasColumnType("decimal(21, 9)")
                    .HasComment("Indica el valor del resultado del análsis del laboratorio.");

                entity.Property(e => e.ValorValidacion).HasComment("Indica el estatus del valor de la validación.");

                entity.HasOne(d => d.Laboratorio)
                    .WithMany(p => p.MuestraValors)
                    .HasForeignKey(d => d.LaboratorioID)
                    .HasConstraintName("FK_MuestraValor_Laboratorio");

                entity.HasOne(d => d.Muestra)
                    .WithMany(p => p.MuestraValors)
                    .HasForeignKey(d => d.MuestraID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MuestraValor_Muestra");

                entity.HasOne(d => d.Operacion)
                    .WithMany(p => p.MuestraValors)
                    .HasForeignKey(d => d.OperacionID)
                    .HasConstraintName("FK_MuestraValor_OperacionConsolidado");

                entity.HasOne(d => d.Parametro)
                    .WithMany(p => p.MuestraValors)
                    .HasForeignKey(d => d.ParametroID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MuestraValor_Parametro");

                entity.HasOne(d => d.ParametroMetodo)
                    .WithMany(p => p.MuestraValors)
                    .HasForeignKey(d => d.ParametroMetodoID)
                    .HasConstraintName("FK_MuestraValor_ParametroMetodo");
            });

            modelBuilder.Entity<Muestreador>(entity =>
            {
                entity.ToTable("Muestreador");

                entity.Property(e => e.MuestreadorID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador del Muestreador.");

                entity.Property(e => e.LaboratorioSignatarioID).HasComment("Identificador del Signatario del Laboratorio.");

                entity.Property(e => e.MuestraID).HasComment("Identificador de la muestra.");

                entity.HasOne(d => d.LaboratorioSignatario)
                    .WithMany(p => p.Muestreadors)
                    .HasForeignKey(d => d.LaboratorioSignatarioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Muestreador_LaboratorioSignatario");

                entity.HasOne(d => d.Muestra)
                    .WithMany(p => p.Muestreadors)
                    .HasForeignKey(d => d.MuestraID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Muestreador_Muestra");
            });

            modelBuilder.Entity<Municipio>(entity =>
            {
                entity.HasKey(e => new { e.MunicipioID, e.EstadoID });

                entity.ToTable("Municipio");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Municipios)
                    .HasForeignKey(d => d.EstadoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Municipio_Estado");
            });

            modelBuilder.Entity<NivelConfiguracion>(entity =>
            {
                entity.HasKey(e => e.NivelID);

                entity.ToTable("NivelConfiguracion");

                entity.Property(e => e.NivelID).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Norma>(entity =>
            {
                entity.ToTable("Norma");

                entity.Property(e => e.NormaID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador de la Norma.");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica el estatus de la norma, señala si la norma está activa o no.");

                entity.Property(e => e.ClienteID).HasComment("Identificador de cliente.");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasComment("Indica una pequeña descripción de la norma.");

                entity.Property(e => e.Evaluacion)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si se realiza la evaluación de la norma para la entrega de resultado, si es falso entonces entrega los valores de todos los parámetros.");

                entity.Property(e => e.ExentaParametros)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si la norma puede exentar parámetros.");

                entity.Property(e => e.FechaFinal)
                    .HasColumnType("datetime")
                    .HasComment("Indica la fecha fin de la vigencia de la norma.");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("date")
                    .HasComment("Indica la fecha de inicio de la vigencia de la norma.");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Indica la fecha de registro de la norma.");

                entity.Property(e => e.MuestrasMultiple).HasDefaultValueSql("((0))");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Indica el nombre de la norma.");

                entity.Property(e => e.RequiereCaudal).HasComment("Indica si la norma requiere del dato de caudal.");

                entity.Property(e => e.RequiereCompuesta)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica sí la norma compone muestra compuesta.");

                entity.Property(e => e.RequiereMensual)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica sí la norma compone informe mensual.");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Normas)
                    .HasForeignKey(d => d.ClienteID)
                    .HasConstraintName("FK_Norma_Cliente");

                entity.HasOne(d => d.NormaPadre)
                    .WithMany(p => p.InverseNormaPadre)
                    .HasForeignKey(d => d.NormaPadreID)
                    .HasConstraintName("FK_Norma_NormaPadre");
            });

            modelBuilder.Entity<NormaConfiguracion>(entity =>
            {
                entity.ToTable("NormaConfiguracion");

                entity.Property(e => e.NormaConfiguracionID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador de la Configuración de la Norma.");

                entity.Property(e => e.AceptaValor)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si la configuración de la norma acepta valor o no.");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica el estatus de la norma, señalando si está Activa o no.");

                entity.Property(e => e.CuerpoReceptorID).HasComment("Identificador del cuerpo receptor.");

                entity.Property(e => e.EscalaDecimal).HasComment("Indica la escala de cuantos decimales debe tener.");

                entity.Property(e => e.Etiqueta)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Exenta).HasComment("Indica si el parámetro es obligatorio para la norma (false - es obligatorio, true - no es obligatorio)");

                entity.Property(e => e.FechaDesactivacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Indica la Fecha de Desactivación de la Norma.");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Indica la Fecha de Registro de la Norma.");

                entity.Property(e => e.Grupo).HasComment("Indica si el Parámetro capturado es un grupo calculado");

                entity.Property(e => e.LCMC).HasComment("Indica si la configuración de la norma tiene LC/MC.");

                entity.Property(e => e.LimiteInferior)
                    .HasColumnType("decimal(18, 9)")
                    .HasComment("Hace referencia al límite inferior permisible de la norma.");

                entity.Property(e => e.LimiteInferiorLFD)
                    .HasColumnType("decimal(18, 9)")
                    .HasComment("Hace referencia al límite inferior permisible de la norma.");

                entity.Property(e => e.LimiteSuperior)
                    .HasColumnType("decimal(18, 9)")
                    .HasComment("Hace referencia al límite superior permisible de la norma.");

                entity.Property(e => e.LimiteSuperiorLFD)
                    .HasColumnType("decimal(18, 9)")
                    .HasComment("Hace referencia al límite superior permisible de la norma.");

                //entity.Property(e => e.MaxAceptado).HasColumnType("decimal(18, 9)");

                //entity.Property(e => e.MinAceptado).HasColumnType("decimal(18, 9)");

                entity.Property(e => e.NivelID).HasComment("Indica en que nivel pertenece el parametro (Simple, Compuest o Mensual).");

                entity.Property(e => e.NormaConfiguracionPadreID).HasComment("Identificador padre de la Configuración de la Norma para Grupo de Parámetros.");

                entity.Property(e => e.NormaID).HasComment("Identificador de la norma.");

                entity.Property(e => e.OperacionID).HasComment("Indica sí el parámetro es calculado de su antecesor.");

                entity.Property(e => e.Orden).HasComment("Indica el orden en el que se visualiza el registro");

                entity.Property(e => e.ParametroID).HasComment("Identificador del Parámetro.");

                entity.Property(e => e.UsoAguaID).HasComment("Identificador del tipo de uso de agua.");

                entity.HasOne(d => d.CuerpoReceptor)
                    .WithMany(p => p.NormaConfiguracions)
                    .HasForeignKey(d => d.CuerpoReceptorID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NormaConfiguracion_CuerpoReceptor");

                entity.HasOne(d => d.Nivel)
                    .WithMany(p => p.NormaConfiguracions)
                    .HasForeignKey(d => d.NivelID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NormaConfiguracion_NivelConfiguracion");

                entity.HasOne(d => d.NormaConfiguracionPadre)
                    .WithMany(p => p.InverseNormaConfiguracionPadre)
                    .HasForeignKey(d => d.NormaConfiguracionPadreID)
                    .HasConstraintName("FK_NormaConfiguracion_NormaConfiguracion");

                entity.HasOne(d => d.Norma)
                    .WithMany(p => p.NormaConfiguracions)
                    .HasForeignKey(d => d.NormaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NormaConfiguracion_Norma");

                entity.HasOne(d => d.Operacion)
                    .WithMany(p => p.NormaConfiguracions)
                    .HasForeignKey(d => d.OperacionID)
                    .HasConstraintName("FK_NormaConfiguracion_OperacionConsolidado");

                entity.HasOne(d => d.Parametro)
                    .WithMany(p => p.NormaConfiguracions)
                    .HasForeignKey(d => d.ParametroID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NormaConfiguracion_Parametro");

                entity.HasOne(d => d.UsoAgua)
                    .WithMany(p => p.NormaConfiguracions)
                    .HasForeignKey(d => d.UsoAguaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NormaConfiguracion_CatUsoAgua");
            });

            modelBuilder.Entity<OperacionConsolidado>(entity =>
            {
                entity.HasKey(e => e.OperacionID);

                entity.ToTable("OperacionConsolidado");

                entity.Property(e => e.OperacionID).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Operador>(entity =>
            {
                entity.ToTable("Operador");

                entity.Property(e => e.OperadorID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador de la tabla de Operador, que hace referencia a los operadores matemáticos.");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Indica el nombre del operador matemático.");

                entity.Property(e => e.Simbolo)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasComment("Indica el símbolo del operador.");
            });

            modelBuilder.Entity<OrdenTrabajo>(entity =>
            {
                entity.ToTable("OrdenTrabajo");

                entity.HasIndex(e => new { e.ClienteID, e.NormaID, e.CuerpoReceptorID, e.UsoAguaID, e.AnexoID }, "IDX_OrdenTrabajo");

                entity.HasIndex(e => new { e.ClienteID, e.FrecuenciaMuestreoID, e.EstatusID, e.NormaID, e.CuerpoReceptorID, e.UsoAguaID, e.FechaEmision, e.NumeroInforme, e.AnexoID, e.ConsecionID, e.LaboratorioID }, "IDX_OrdenTrabajoRow");

                entity.Property(e => e.OrdenTrabajoID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador de la tabla de Orden del Trabajo.");

                entity.Property(e => e.AnexoID).HasComment("Indentificador que hace referencia al Anexo del Análisis.");

                entity.Property(e => e.Anio).HasComment("Indica el año de la orden de trabajo.");

                entity.Property(e => e.ClienteID).HasComment("Identificador del Cliente.");

                entity.Property(e => e.Consecutivo).HasComment("Indica el consecutivo del año.");

                entity.Property(e => e.CuerpoReceptorID).HasComment("Identificador del cuerpo receptor.");

                entity.Property(e => e.FechaEmision)
                    .HasColumnType("date")
                    .HasComment("Indica la Fecha de Emisión de la Orden de Trabajo.");

                entity.Property(e => e.Folio)
                    .IsRequired()
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasComment("Indica el Folio de la Orden de Trabajo.");

                entity.Property(e => e.FrecuenciaMuestreoID).HasComment("Indica el identificador de la frecuencia de muestreo.");

                entity.Property(e => e.HorasProceso).HasComment("Indica las horas del proceso.");

                entity.Property(e => e.NormaID).HasComment("Identificador de la norma.");

                entity.Property(e => e.NumeroInforme)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Indica el número que de forma interna maneja el laboratorio para sus informes");

                entity.Property(e => e.UsoAguaID).HasComment("Identificador del tipo de uso de agua.");

                entity.HasOne(d => d.Anexo)
                    .WithMany(p => p.OrdenTrabajos)
                    .HasForeignKey(d => d.AnexoID)
                    .HasConstraintName("FK_OrdenTrabajo_Anexo");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.OrdenTrabajos)
                    .HasForeignKey(d => d.ClienteID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrdenTrabajo_Cliente");

                entity.HasOne(d => d.Consecion)
                    .WithMany(p => p.OrdenTrabajos)
                    .HasForeignKey(d => d.ConsecionID)
                    .HasConstraintName("FK_OrdenTrabajo_Consecion");

                entity.HasOne(d => d.CuerpoReceptor)
                    .WithMany(p => p.OrdenTrabajos)
                    .HasForeignKey(d => d.CuerpoReceptorID)
                    .HasConstraintName("FK_OrdenTrabajo_CuerpoReceptor");

                entity.HasOne(d => d.Estatus)
                    .WithMany(p => p.OrdenTrabajos)
                    .HasForeignKey(d => d.EstatusID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrdenTrabajo_Estatus");

                entity.HasOne(d => d.FrecuenciaMuestreo)
                    .WithMany(p => p.OrdenTrabajos)
                    .HasForeignKey(d => d.FrecuenciaMuestreoID)
                    .HasConstraintName("FK_OrdenTrabajo_FrecuenciaMuestreo");

                entity.HasOne(d => d.Laboratorio)
                    .WithMany(p => p.OrdenTrabajos)
                    .HasForeignKey(d => d.LaboratorioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrdenTrabajo_Laboratorio");

                entity.HasOne(d => d.Norma)
                    .WithMany(p => p.OrdenTrabajos)
                    .HasForeignKey(d => d.NormaID)
                    .HasConstraintName("FK_OrdenTrabajo_Norma");

                entity.HasOne(d => d.UsoAgua)
                    .WithMany(p => p.OrdenTrabajos)
                    .HasForeignKey(d => d.UsoAguaID)
                    .HasConstraintName("FK_OrdenTrabajo_CatUsoAgua");
            });

            modelBuilder.Entity<Parametro>(entity =>
            {
                entity.ToTable("Parametro");

                entity.Property(e => e.ParametroID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador del  Parámetro.");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica el estatus del parametro, señalando si está Activo o no.");

                entity.Property(e => e.Descripcion)
                    .IsUnicode(false)
                    .HasComment("Indica una breve descripción del parámetro.");

                entity.Property(e => e.FechaFinal)
                    .HasColumnType("datetime")
                    .HasComment("Indica la Fecha Final de Operativa del parámetro.");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("date")
                    .HasComment("Indica la Fecha de Inicio Operativa del Parámetro.");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Indica la Fecga de Registro del Parámetro.");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasComment("Nombre del Parámetro.");

                entity.Property(e => e.UnidadMedidaID).HasComment("Identificador de la unidad de medida empleada.");

                entity.HasOne(d => d.UnidadMedida)
                    .WithMany(p => p.Parametros)
                    .HasForeignKey(d => d.UnidadMedidaID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Parametro_CatUnidadMedida");
            });

            modelBuilder.Entity<ParametroMetodo>(entity =>
            {
                entity.ToTable("ParametroMetodo");

                entity.Property(e => e.ParametroMetodoID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador de la relación entre parámetro - métdo.");

                entity.Property(e => e.Eliminado)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Identificador s� es eliminado.");

                entity.Property(e => e.FechaEliminacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de eliminaci�n.");

                entity.Property(e => e.MetodoID).HasComment("Identificador de Método.");

                entity.Property(e => e.ParametroID).HasComment("Indica el identificador del Parámetro.");

                entity.HasOne(d => d.Metodo)
                    .WithMany(p => p.ParametroMetodos)
                    .HasForeignKey(d => d.MetodoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ParametroMetodo_Metodo");

                entity.HasOne(d => d.Parametro)
                    .WithMany(p => p.ParametroMetodos)
                    .HasForeignKey(d => d.ParametroID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ParametroMetodo_Parametro");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.ToTable("Persona");

                entity.Property(e => e.PersonaID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador de la persona.");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si la persona esta en estado Activo o Inactivo.");

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasComment("Indica el apellido materno de la persona.");

                entity.Property(e => e.ApellidoPaterno)
                    .IsRequired()
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasComment("Indica el apellido paterno de la persona.");

                entity.Property(e => e.CURP)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Indica el CURP de la persona.");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Hace referencia a la fecha en que se registró a la persona.");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasComment("Indica el nombre de la persona.");

                entity.Property(e => e.RFC)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasComment("Indica el RFC de la persona.");
            });

            modelBuilder.Entity<SolicitudCancelacion>(entity =>
            {
                entity.ToTable("SolicitudCancelacion");

                entity.Property(e => e.SolicitudCancelacionID).HasComment("Identificador del la solicitud de cancelaci�n.");

                entity.Property(e => e.EstatusID).HasComment("Identificador del Estatus.");

                entity.Property(e => e.FechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en la que se registra la aprobaci�n o rechazo de la solicitud.");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en la que se registra la solictud.");

                entity.Property(e => e.FirmaAprobacion)
                    .IsUnicode(false)
                    .HasComment("Firmado electr�nico de la aprobaci�n o rechazo.");

                entity.Property(e => e.FirmaSolicitud)
                    .IsUnicode(false)
                    .HasComment("Firmado electr�nico de la solicitud.");

                entity.Property(e => e.LaboratorioID).HasComment("Identificador del Laboratorio solicitante.");

                entity.Property(e => e.Motivo)
                    .IsRequired()
                    .HasMaxLength(3500)
                    .IsUnicode(false)
                    .HasComment("Motivo por el cu�l se solicita la cancelaci�n.");

                entity.HasOne(d => d.Estatus)
                    .WithMany(p => p.SolicitudCancelacions)
                    .HasForeignKey(d => d.EstatusID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SolicitudCancelacion_Estatus");

                entity.HasOne(d => d.Laboratorio)
                    .WithMany(p => p.SolicitudCancelacions)
                    .HasForeignKey(d => d.LaboratorioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SolicitudCancelacion_Laboratorio");
            });

            modelBuilder.Entity<SolicitudCancelacionDetalle>(entity =>
            {
                entity.ToTable("SolicitudCancelacionDetalle");

                entity.Property(e => e.SolicitudCancelacionDetalleID).HasComment("Identificador del detalle la solicitud de cancelaci�n.");

                entity.Property(e => e.AnalisisID).HasComment("Identificador del An�lisis.");

                entity.Property(e => e.SolicitudCancelacionID).HasComment("Identificador de la solicitud.");

                entity.HasOne(d => d.Analisis)
                    .WithMany(p => p.SolicitudCancelacionDetalles)
                    .HasForeignKey(d => d.AnalisisID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SolicitudCancelacionDetalle_Analisis");

                entity.HasOne(d => d.SolicitudCancelacion)
                    .WithMany(p => p.SolicitudCancelacionDetalles)
                    .HasForeignKey(d => d.SolicitudCancelacionID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SolicitudCancelacionDetalle_SolicitudCancelacion");
            });

            modelBuilder.Entity<TBitacora>(entity =>
            {
                entity.HasKey(e => e.Idrow);

                entity.ToTable("TBitacora");

                entity.Property(e => e.Idrow).ValueGeneratedNever();

                entity.Property(e => e.CadenaFirma).IsUnicode(false);

                entity.Property(e => e.FechaMod).HasColumnType("datetime");

                entity.Property(e => e.Firma).IsUnicode(false);

                entity.Property(e => e.ModuloMod)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NSerieCert)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NUser)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoMov)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");

                entity.Property(e => e.UsuarioID)
                    .ValueGeneratedNever()
                    .HasComment("Identificador de la tabla de Usuario.");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Estatus del usuario.");

                entity.Property(e => e.Contrasenia)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Contraseña del usuario para acceder al sistema.");

                entity.Property(e => e.Correo)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasComment("Correo electrónico del usuario.");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Indica la fecha en que se registó el usuario.");

                entity.Property(e => e.LaboratorioID).HasComment("Identificador único del Laboratorio.");

                entity.Property(e => e.PersonaID).HasComment("Identificador único que hace referencia a la persona.");

                entity.Property(e => e.Usuario1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Usuario")
                    .HasComment("Nombre de usuario que se usará para acceder al sistema.");

                entity.Property(e => e.UsuarioRolID).HasComment("Identificador del rol del usuairo.");

                entity.HasOne(d => d.Laboratorio)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.LaboratorioID)
                    .HasConstraintName("FK_Usuario_Laboratorio");

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.PersonaID)
                    .HasConstraintName("FK_Usuario_Persona");

                entity.HasOne(d => d.UsuarioRol)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.UsuarioRolID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_CatUsuarioRol");
            });

            modelBuilder.Entity<VW_BASE_GLOBAL_ANALISIS_SGAA>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VW_BASE_GLOBAL_ANALISIS_SGAA");

                entity.Property(e => e.ANEXO)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ASENTAMIENTO_HUMANO)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ASENTAMIENTO HUMANO");

                entity.Property(e => e.CALLE)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CAUDAL).HasColumnType("decimal(18, 9)");

                entity.Property(e => e.CLIENTE)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CUERPO_RECEPTOR)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CUERPO RECEPTOR");

                entity.Property(e => e.CÓDGIO_POSTAL).HasColumnName("CÓDGIO POSTAL");

                entity.Property(e => e.ESTADO)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FECHA_DE_CONFORMACIÓN)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA DE CONFORMACIÓN");

                entity.Property(e => e.FECHA_DE_FIRMA)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA DE FIRMA");

                entity.Property(e => e.HORAS_PROCESO).HasColumnName("HORAS PROCESO");

                entity.Property(e => e.INFORME)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LABORATORIO)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.LATITUD).HasColumnType("decimal(18, 14)");

                entity.Property(e => e.LC_MC)
                    .HasColumnType("decimal(21, 9)")
                    .HasColumnName("LC/MC");

                entity.Property(e => e.LOCALIDAD)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LONGITUD).HasColumnType("decimal(18, 14)");

                entity.Property(e => e.MUNICIPIO)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MÉTODO)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.NORMA)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre_de_Cuerpo_Receptor)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("Nombre de Cuerpo Receptor");

                entity.Property(e => e.NÚMERO_EXTERIOR)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NÚMERO EXTERIOR");

                entity.Property(e => e.NÚMERO_INTERIOR)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NÚMERO INTERIOR");

                entity.Property(e => e.PARÁMETRO)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.RESULTADO).HasColumnType("decimal(21, 9)");

                entity.Property(e => e.RFC)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.TIPO_ASENTAMIENTO_HUMANO)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIPO ASENTAMIENTO HUMANO");

                entity.Property(e => e.TIPO_DE_MUESTRA)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIPO DE MUESTRA");

                entity.Property(e => e.TÍTULO)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.UNIDAD)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.USO_DE_AGUA)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("USO DE AGUA");
            });

            modelBuilder.Entity<Vw_AnalisisMensual>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vw_AnalisisMensual");

                entity.Property(e => e.Anexo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Caudal).HasColumnType("decimal(18, 9)");

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Estatus_del_análisis)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("Estatus del análisis");

                entity.Property(e => e.Está_habilitado_)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("Está habilitado ");

                entity.Property(e => e.Exento)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha_de_conformación)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha de conformación");

                entity.Property(e => e.Fecha_de_emisión)
                    .HasColumnType("date")
                    .HasColumnName("Fecha de emisión");

                entity.Property(e => e.Fecha_de_firma)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha de firma");

                entity.Property(e => e.Fecha_de_recepción_en_el_laboratorio)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha de recepción en el laboratorio");

                entity.Property(e => e.Firmada)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Folio_CONAGUA).HasColumnName("Folio CONAGUA");

                entity.Property(e => e.Horas_proceso).HasColumnName("Horas proceso");

                entity.Property(e => e.Informe)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Laboratorio_que_analizó)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Laboratorio que analizó");

                entity.Property(e => e.Laboratorio_que_realiza_el_muestreo)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Laboratorio que realiza el muestreo");

                entity.Property(e => e.Laboratorio_que_registra_la_OT)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Laboratorio que registra la OT");

                entity.Property(e => e.Lc_Mc)
                    .HasColumnType("decimal(21, 9)")
                    .HasColumnName("Lc/Mc");

                entity.Property(e => e.Muestreador)
                    .HasMaxLength(212)
                    .IsUnicode(false);

                entity.Property(e => e.Municipio)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Método)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.No_analizado)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("No analizado");

                entity.Property(e => e.Nombre_usuario)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Nombre usuario");

                entity.Property(e => e.Norma)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Parámetro)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.RFC_Laboratorio)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("RFC Laboratorio");

                entity.Property(e => e.RFC_usuario)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("RFC usuario");

                entity.Property(e => e.Resultado).HasColumnType("decimal(21, 9)");

                entity.Property(e => e.Tipo_de_cuerpo_receptor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Tipo de cuerpo receptor");

                entity.Property(e => e.Tipo_muestra)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Tipo muestra");

                entity.Property(e => e.Título)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Unidad)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Uso_del_cuerpo_receptor)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Uso del cuerpo receptor");
            });

            modelBuilder.Entity<Vw_BusquedaAnalisi>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vw_BusquedaAnalisis");

                entity.Property(e => e.Anexo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Contribuyente)
                    .IsRequired()
                    .HasMaxLength(392)
                    .IsUnicode(false);

                entity.Property(e => e.Estatus)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.FechaConformacion).HasColumnType("datetime");

                entity.Property(e => e.FechaEmision).HasColumnType("date");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Firmada)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Laboratorio)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Norma)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroInforme)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RfcContribuyente)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.RfcLaboratorio)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Vw_BusquedaClienteAnalisi>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vw_BusquedaClienteAnalisis");

                entity.Property(e => e.Anexo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Estatus)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.FechaEmision).HasColumnType("date");

                entity.Property(e => e.Firmada)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.NomLab)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NombreCliente)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NombreInforme)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Norma)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RFCCliente)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.RFCLaboratorio)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.fechaFirmaAnalisis).HasColumnType("datetime");

                entity.Property(e => e.nombreCuerpoReceptor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Vw_Conamer>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vw_Conamer");

                entity.Property(e => e.Anexo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Caudal).HasColumnType("decimal(18, 9)");

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Estatus_del_análisis)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("Estatus del análisis");

                entity.Property(e => e.Está_habilitado_)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("Está habilitado ");

                entity.Property(e => e.Exento)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha_de_conformación)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha de conformación");

                entity.Property(e => e.Fecha_de_emisión)
                    .HasColumnType("date")
                    .HasColumnName("Fecha de emisión");

                entity.Property(e => e.Fecha_de_firma)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha de firma");

                entity.Property(e => e.Fecha_de_recepción_en_el_laboratorio)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha de recepción en el laboratorio");

                entity.Property(e => e.Firmada)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Folio_CONAGUA).HasColumnName("Folio CONAGUA");

                entity.Property(e => e.Horas_proceso).HasColumnName("Horas proceso");

                entity.Property(e => e.Informe)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Laboratorio_que_analizó)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Laboratorio que analizó");

                entity.Property(e => e.Laboratorio_que_realiza_el_muestreo)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Laboratorio que realiza el muestreo");

                entity.Property(e => e.Laboratorio_que_registra_la_OT)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Laboratorio que registra la OT");

                entity.Property(e => e.Lc_Mc)
                    .HasColumnType("decimal(21, 9)")
                    .HasColumnName("Lc/Mc");

                entity.Property(e => e.Muestreador)
                    .HasMaxLength(212)
                    .IsUnicode(false);

                entity.Property(e => e.Municipio)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Método)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.No_analizado)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("No analizado");

                entity.Property(e => e.Nombre_usuario)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Nombre usuario");

                entity.Property(e => e.Norma)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Parámetro)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.RFC_Laboratorio)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("RFC Laboratorio");

                entity.Property(e => e.RFC_usuario)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("RFC usuario");

                entity.Property(e => e.Resultado).HasColumnType("decimal(21, 9)");

                entity.Property(e => e.Tipo_de_cuerpo_receptor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Tipo de cuerpo receptor");

                entity.Property(e => e.Tipo_muestra)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Tipo muestra");

                entity.Property(e => e.Título)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Unidad)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Uso_del_cuerpo_receptor)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Uso del cuerpo receptor");
            });

            modelBuilder.Entity<Vw_Habilitar_Anali>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vw_Habilitar_Analis");

                entity.Property(e => e.Anexo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Estatus_del_análisis)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("Estatus del análisis");

                entity.Property(e => e.Fecha_de_conformación)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha de conformación");

                entity.Property(e => e.Fecha_de_firma)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha de firma");

                entity.Property(e => e.Firmada)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Firmado_fuera_de_tiempo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("Firmado fuera de tiempo");

                entity.Property(e => e.Informe)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NombreContribuyente)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NombreLaboratorio)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Norma)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RFCContribuyente)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.RFCLaboratorio)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Vw_Ot_Grid>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vw_Ot_Grid");

                entity.Property(e => e.Anexo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Estatus)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.FechaConformacion).HasColumnType("datetime");

                entity.Property(e => e.FechaEmision).HasColumnType("date");

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Firmada)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Informe)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NombreLaboratorio)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Norma)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RFC)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.RFCLaboratorio)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Vw_Reporte_Analis_ControlObligacione>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vw_Reporte_Analis_ControlObligaciones");

                entity.Property(e => e.ANEXO)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CURP_cliente)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cuerpo_receptor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Cuerpo receptor");

                entity.Property(e => e.Estatus_del_análisis)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("Estatus del análisis");

                entity.Property(e => e.FechaEmision).HasColumnType("date");

                entity.Property(e => e.FechaFirma).HasColumnType("datetime");

                entity.Property(e => e.Fecha_Registro_Analisis)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha Registro Analisis");

                entity.Property(e => e.Fecha_conformación)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha conformación");

                entity.Property(e => e.Firmada)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Folio_CONAGUA).HasColumnName("Folio CONAGUA");

                entity.Property(e => e.Laboratorio)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NOMBRE_cliente)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NORMA)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroInforme)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RFC_cliente)
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.RFC_laboratorio)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.TITULO)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Uso_de_agua)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Uso de agua");
            });

            modelBuilder.Entity<Vw_Reporte_titulo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vw_Reporte_titulos");

                entity.Property(e => e.Anexo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Caudal).HasColumnType("decimal(18, 9)");

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Estatus_del_análisis)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("Estatus del análisis");

                entity.Property(e => e.Está_habilitado_)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("Está habilitado ");

                entity.Property(e => e.Exento)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha_de_conformación)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha de conformación");

                entity.Property(e => e.Fecha_de_emisión)
                    .HasColumnType("date")
                    .HasColumnName("Fecha de emisión");

                entity.Property(e => e.Fecha_de_firma)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha de firma");

                entity.Property(e => e.Fecha_de_recepción_en_el_laboratorio)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha de recepción en el laboratorio");

                entity.Property(e => e.Firmada)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Folio_CONAGUA).HasColumnName("Folio CONAGUA");

                entity.Property(e => e.Horas_proceso).HasColumnName("Horas proceso");

                entity.Property(e => e.Informe)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Laboratorio_que_analizó)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Laboratorio que analizó");

                entity.Property(e => e.Laboratorio_que_realiza_el_muestreo)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Laboratorio que realiza el muestreo");

                entity.Property(e => e.Laboratorio_que_registra_la_OT)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Laboratorio que registra la OT");

                entity.Property(e => e.Lc_Mc)
                    .HasColumnType("decimal(21, 9)")
                    .HasColumnName("Lc/Mc");

                entity.Property(e => e.Muestreador)
                    .HasMaxLength(212)
                    .IsUnicode(false);

                entity.Property(e => e.Municipio)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Método)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.No_analizado)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("No analizado");

                entity.Property(e => e.Nombre_usuario)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Nombre usuario");

                entity.Property(e => e.Norma)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Parámetro)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.RFC_Laboratorio)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("RFC Laboratorio");

                entity.Property(e => e.RFC_usuario)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("RFC usuario");

                entity.Property(e => e.Resultado).HasColumnType("decimal(21, 9)");

                entity.Property(e => e.Tipo_de_cuerpo_receptor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Tipo de cuerpo receptor");

                entity.Property(e => e.Tipo_muestra)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Tipo muestra");

                entity.Property(e => e.Título)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Unidad)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Uso_del_cuerpo_receptor)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Uso del cuerpo receptor");
            });

            modelBuilder.Entity<vw_LaboratoriosPALA>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_LaboratoriosPALA");

                entity.Property(e => e.AcreditacionEMA)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.AcreditacionFechaFin).HasColumnType("date");

                entity.Property(e => e.AcreditacionFechaInicio).HasColumnType("date");

                entity.Property(e => e.Aprobacion)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.AprobacionFechaFin).HasColumnType("date");

                entity.Property(e => e.AprobacionFechaInicio).HasColumnType("date");

                entity.Property(e => e.Calle)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Colonia)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Contacto)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Laboratorio)
                    .HasMaxLength(392)
                    .IsUnicode(false);

                entity.Property(e => e.Metodo)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Municipio)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroExterior)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroInterior)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Parametro)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.RFC)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Representante)
                    .HasMaxLength(212)
                    .IsUnicode(false);

                entity.Property(e => e.TecnicaAnalitica).IsUnicode(false);

                entity.Property(e => e.TipoContacto)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<vw_reporte_SDT>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_reporte_SDT");

                entity.Property(e => e.Anexo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Estatus_del_análisis)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("Estatus del análisis");

                entity.Property(e => e.Fecha_de_conformación)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha de conformación");

                entity.Property(e => e.Firmada)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre_del_contribuyente)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Nombre del contribuyente");

                entity.Property(e => e.RFC)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Título)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Valor).HasColumnType("decimal(21, 9)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
