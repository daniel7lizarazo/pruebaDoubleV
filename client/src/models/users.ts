export interface UsuariosResponse
{
  id: number
  user: string
  fechaCreacion: string
}

export interface UsuarioRequest
{
  user: string
  pass: string
}
