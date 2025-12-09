using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext _context;
        private readonly DbSet<T> _entity;

        public GenericRepository(DataContext context)
        {
            _context = context;
            _entity = _context.Set<T>();
        }

        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _entity.AsQueryable();

            return new ActionResponse<IEnumerable<T>>
            {
                WasSuccesse = true,
                Result = await queryable
                    .Skip((pagination.Page - 1) * pagination.RecordsNumber)
                    .Take(pagination.RecordsNumber)
                    .ToListAsync()
            };
        }

        public virtual async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            double count = await _entity.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);

            return new ActionResponse<int>
            {
                WasSuccesse = true,
                Result = totalPages
            };
        }

        public virtual async Task<ActionResponse<T>> AddAsync(T entity)
        {
            _context.Add(entity);
            try
            {
                await _context.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    WasSuccesse = true,
                    Result = entity
                };
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException!.Message.Contains("duplicate"))
                    {
                        return DbUpdateExceptionActionResponse();
                    }
                }
                return new ActionResponse<T>
                {
                    WasSuccess = false,
                    Message = ex.Message
                };
            }
            catch (Exception exception)
            {
                return ExceptionActionResponse(exception);
            }
        }

        public virtual async Task<ActionResponse<T>> DeleteAsync(int id)
        {
            var row = await _entity.FindAsync(id);
            if (row == null)
            {
                return new ActionResponse<T>
                {
                    WasSuccesse = false,
                    Message = "Registro no encontrado "
                };
            }
            try
            {
                _entity.Remove(row);
                await _context.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    WasSuccesse = true
                };
            }
            catch
            {
                return new ActionResponse<T>
                {
                    WasSuccesse = false,
                    Message = "No se puede borrar, porque tiene registros relacionados"
                };
            }
        }

        public virtual async Task<ActionResponse<T>> GetAsync(int id)
        {
            var row = await _entity.FindAsync(id);
            if (row == null)
            {
                return new ActionResponse<T>
                {
                    WasSuccesse = false,
                    Message = "Registro no encontrado "
                };
            }
            return new ActionResponse<T>
            {
                WasSuccesse = true,
                Result = row
            };
        }

        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync()
        {
            return new ActionResponse<IEnumerable<T>>
            {
                WasSuccesse = true,
                Result = await _entity.ToListAsync()
            };
        }

        public virtual async Task<ActionResponse<T>> UpdateAsync(T entity)
        {
            _context.Update(entity);
            try
            {
                await _context.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    WasSuccesse = true,
                    Result = entity
                };
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException!.Message.Contains("duplicate"))
                    {
                        return DbUpdateExceptionActionResponse();
                    }
                }
                return new ActionResponse<T>
                {
                    WasSuccess = false,
                    Message = ex.Message
                };
            }
            catch (Exception exception)
            {
                return ExceptionActionResponse(exception);
            }
        }

        private static ActionResponse<T> DbUpdateExceptionActionResponse()
        {
            return new ActionResponse<T>
            {
                WasSuccesse = false,
                Message = "Ya existe el registro que estás intentando crear"
            };
        }

        private static ActionResponse<T> ExceptionActionResponse(Exception exception)
        {
            return new ActionResponse<T>
            {
                WasSuccesse = false,
                Message = exception.Message
            };
        }
    }
}