namespace IdeaBox.Api.DTOs;

// Retourné dans la liste (sans le contenu complet)
public class IdeeListDto
{
    public int IdIdee { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string Auteur { get; set; } = string.Empty;
    public string Priorite { get; set; } = string.Empty;
    public string Difficulte { get; set; } = string.Empty;
    public int NbCommentaires { get; set; }
    public int NbVotes { get; set; }
    public DateTime DateCreation { get; set; }
}

// Retourné dans le détail (avec contenu + commentaires)
public class IdeeDetailDto
{
    public int IdIdee { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string Contenu { get; set; } = string.Empty;
    public string Auteur { get; set; } = string.Empty;
    public string Priorite { get; set; } = string.Empty;
    public string Difficulte { get; set; } = string.Empty;
    public DateTime DateCreation { get; set; }
    public DateTime DateModification { get; set; }
    public List<CommentaireDto> Commentaires { get; set; } = new();
    public int NbVotes { get; set; }
}

// Création d'une idée
public class CreateIdeeDto
{
    public string Titre { get; set; } = string.Empty;
    public string Contenu { get; set; } = string.Empty;
    public string Auteur { get; set; } = string.Empty;
    public string Priorite { get; set; } = "moyenne";
    public string Difficulte { get; set; } = "moyenne";
}

// Mise à jour d'une idée
public class UpdateIdeeDto
{
    public string Titre { get; set; } = string.Empty;
    public string Contenu { get; set; } = string.Empty;
    public string Priorite { get; set; } = "moyenne";
    public string Difficulte { get; set; } = "moyenne";
}

// Commentaire imbriqué dans le détail
public class CommentaireDto
{
    public int IdCommentaire { get; set; }
    public string Contenu { get; set; } = string.Empty;
    public string Auteur { get; set; } = string.Empty;
    public DateTime DateCreation { get; set; }
}

// Création d'un commentaire
public class CreateCommentaireDto
{
    public string Contenu { get; set; } = string.Empty;
    public string Auteur { get; set; } = string.Empty;
}

// Création d'un vote
public class CreateVoteDto
{
    public string Auteur { get; set; } = string.Empty;
}