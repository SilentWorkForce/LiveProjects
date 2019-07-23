class RecipeBasic:
    def __init__(self, id, title, image):
        self.id = id
        self.title = title
        self.image = image

class FullRecipe(RecipeBasic):
    def __init__(self, id, title, image, area, instructions, youtube, ingredients):
        RecipeBasic.__init__(self, id, title, image)
        self.area = area
        self.instructions = instructions
        self.youtube = youtube
        self.ingredients = ingredients

    def embedYouTube(self):
        if "watch?v=" in self.youtube:
            #Change from watch parameter to embed parameter
            self.youtube = self.youtube.replace("watch?v=", "embed/")
            self.youtube += "?controls=1"

        
