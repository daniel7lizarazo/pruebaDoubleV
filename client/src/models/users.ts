export interface PersonaResponse
{
  id: number
  nombres: string
  apellidos: string
  email: string
  numeroIdentificacion: string
  tipoIdentificacion: string
  fechaCreacion: Date
  identificacionCompleta: string
  nombreCompleto: string
}

export interface UsuariosResponse
{
  id: number
  user: string
  fechaCreacion: string
}
