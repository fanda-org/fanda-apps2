import { Subject, Observable } from 'rxjs';

export function asObservable<T>(subject: Subject<any>): Observable<T> {
  return new Observable<T>((fn) => subject.subscribe(fn));
}
