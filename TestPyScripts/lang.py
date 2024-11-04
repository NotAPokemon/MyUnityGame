x = input("DIR: ")

class Line:
    def __init__(self, string: str):
        self.string = string
    def next(self):
        result = self.string[0]
        self.string = self.string[1:]
        return result
    def len(self) -> int:
        return len(self.string)
    def __str__(self):
        return self.string
    def __getattribute__(self, name):
        boolean = True
        names = ["string", "next", "len"]
        for string in names:
                if name == string:
                    boolean = False
        if (boolean):
            return object.__getattribute__(self.string, name)
        return object.__getattribute__(self, name)


with open(x, 'r') as f:
    text = f.read()
    text = text.split(";")
    text2 = []
    for string in text:
        text2.append(Line(string.replace('\n', "")))
        text2.append(Line("[E{}]"))
     
    text = text2[:-1]
    text[-1] = Line("EOF")

class BC:
    def __init__(self, *args):
        pass

class AOC:
    def __init__(self, *args):
        pass

class DEFAULT:
    def __init__(self, *args):
        pass

class S:
    def __init__(self, *args):
        pass
class E:
    def __init__(self, *args):
        pass

class CommandFactory:
    def __init__(self, string):
        self.str = Line(string)
    def toCommand(self):
        name = ""
        t = self.str.next()
        while (t != "{"):
            name += t
            t = self.str.next()
        args = ""
        t = self.str.next()
        while t != "}":
            args += t
            try:
                t = self.str.next()
            except:
                break
        return self.getClass(name)(args.split(","))
    def getClass(self,name):
        cls = globals().get(name)
        if cls is not None:
            return cls
        return DEFAULT

print(text[1])


def tokenize(text) -> list:
    tokens = []
    for i in range(len(text)):
        nextItem = text[i].next()
        while (nextItem == "["):
            t = text[i].next()
            tokens.append("")
            while t != "]":
                tokens[-1] += t
                t = text[i].next()
                if (t == "]"):
                    try:
                        nextItem = text[i].next()
                    except:
                        nextItem = ""
                    break
    return tokens

commands = [CommandFactory(string).toCommand() for string in tokenize(text)]

print(commands[-1].args)