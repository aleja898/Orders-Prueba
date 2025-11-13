using Orders.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;


namespace Orders.Shared.Entities
{
    public class Category : IEntityWithName
    {
        public int Id { get; set; }

        [Display(Name = "Nombre de la categoría")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Required(ErrorMessage = "Error el campo {0} es requerido")]
        public string Name { get; set; } = null!;
    }
}
