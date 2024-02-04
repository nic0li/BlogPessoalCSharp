namespace BlogPessoal.Model;

public class Login
{
    public long Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string NomeDeUsuario { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;

    public string Foto { get; set; } = string.Empty;

    public string Tipo { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;
}
