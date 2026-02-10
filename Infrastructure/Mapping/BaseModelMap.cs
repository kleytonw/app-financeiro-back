using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Models;
using ERP.Domain.Entidades;

namespace ERP.Infrastructure.Mapping
{
    public abstract class BaseModelMap<T> where T : BaseModel
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            #region Auditoria
            builder.Property(c => c.UsuarioInclusao);
            builder.Property(c => c.DataInclusao);
            builder.Property(c => c.UsuarioAlteracao);
            builder.Property(c => c.DataAlteracao);
            builder.Property(c => c.UsuarioExclusao);
            builder.Property(c => c.DataExclusao);
            builder.Property(c => c.Situacao);
            #endregion
        }
    }
}