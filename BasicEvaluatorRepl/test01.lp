3
(+ 4 7)
(set x 4)
(+ x x)
(print x)
(set y 5)
(begin (print x) (print y) (* x y))
(if (> y 0) 5 10)
(while (> y 0) (begin (set x (+ x x)) (set y (- y 1))))
x
(define +1 (x) (+ x 1))
(+1 4)
(define double (x) (+ x x))
(double 4)
x
(define setx (x y) (begin (set x (+ x y)) x))
(setx x 1)
x
(define not (boolval) (if boolval 0 1))
(define <> (x y) (not (= x y)))
(define mod (m n) (- m (* n (/ m n))))
(define gcd (m n)
    (begin
        (set r (mod m n))
        (while (<> r 0)
            (begin
                (set m n)
                (set n r)
                (set r (mod m n))))
        n))
(gcd 6 15)
(define gcd (m n)
    (if (= n 0) m (gcd n (mod m n))))
