grammar BasicEvaluator ;

prog      : input+ ;
input     : expression                                                            # expressionInput
          | funDef                                                                # funDefInput
          ;
funDef    : LPAREN DEFINE function argList expression RPAREN ;
argList   : LPAREN variable* RPAREN ;
expression: value                                                                 # valueExpr
          | variable                                                              # variableExpr
          | LPAREN IF expression expression expression RPAREN                     # ifExpr
          | LPAREN WHILE expression expression RPAREN                             # whileExpr
          | LPAREN SET variable expression RPAREN                                 # setExpr
          | LPAREN BEGIN expression+ RPAREN                                       # beginExpr
          | LPAREN function expression* RPAREN                                    # functionExpr
          | LPAREN op=('+'|'-'|'*'|'/'|'='|'<'|'>') expression expression RPAREN  # operatorExpr
          | LPAREN PRINT expression RPAREN                                        # printExpr
          ;

value     : INTEGER ;
function  : NAME ;
variable  : NAME ;

ADD       : '+' ;
SUB       : '-' ;
MUL       : '*' ;
DIV       : '/' ;
EQ        : '=' ;
LT        : '<' ;
GT        : '>' ;
PRINT     : 'print' ;

LPAREN    : '(' ;
RPAREN    : ')' ;
DEFINE    : 'define' ;
IF        : 'if' ;
WHILE     : 'while' ;
SET       : 'set' ;
BEGIN     : 'begin' ;

INTEGER   : [-]? [0-9]+ ;
// NAME: match any sequence of characters not an integer nor containing blank, (, ), or ;
NAME      : ~[ ();\t\r\n]+ ;
COMMENT   : ';' ~[\r\n]* -> skip ;
WS        : [ \t\r\n]+ -> skip ;