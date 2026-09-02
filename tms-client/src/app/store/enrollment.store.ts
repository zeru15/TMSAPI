import { computed, inject } from '@angular/core';

import {
  signalStore,
  withComputed,
  withMethods,
  patchState,
  withState,
} from '@ngrx/signals';

import {
  withEntities,
  setAllEntities,
  updateEntity,
} from '@ngrx/signals/entities';

import { rxMethod } from '@ngrx/signals/rxjs-interop';

import {
  pipe,
  concatMap,
  tap,
  catchError,
  EMPTY,
} from 'rxjs';

import { EnrollmentService } from '../services/enrollment.service';
import { Enrollment } from '../models/enrollment.model';

export const EnrollmentStore = signalStore(

  { providedIn: 'root' },

  withState({
    isLoading: false,
    error: null as string | null,
  }),

  withEntities<Enrollment>(),

  withComputed((store) => ({
    pendingCount: computed(() =>
      store
        .entities()
        .filter(
          enrollment => enrollment.status === 'Pending'
        ).length
    ),
  })),

  withMethods(
    (
      store,
      api = inject(EnrollmentService)
    ) => ({

      loadEnrollments: rxMethod<void>(
        pipe(

          tap(() => {
            patchState(store, {
              isLoading: true,
              error: null,
            });
          }),

          concatMap(() =>
            api.getAll().pipe(

              tap(rows => {
                patchState(
                  store,
                  setAllEntities(rows),
                  {
                    isLoading: false,
                  }
                );
              }),

              catchError(error => {

                console.error(error);

                patchState(store, {
                  isLoading: false,
                  error: 'Failed to load enrollments.',
                });

                return EMPTY;
              })
            )
          )
        )
      ),

      approveEnrollment: rxMethod<string>(
        pipe(

          tap(id => {

            patchState(
              store,

              updateEntity({
                id,
                changes: {
                  status: 'Approved',
                },
              })
            );

          }),

          concatMap(id =>
            api.approve(id).pipe(

              tap(() => {
                console.log(
                  `Enrollment ${id} approved.`
                );
              }),

              catchError(error => {

                console.error(error);

                patchState(
                  store,

                  updateEntity({
                    id,
                    changes: {
                      status: 'Pending',
                    },
                  })
                );

                patchState(store, {
                  error: 'Failed to approve enrollment.',
                });

                return EMPTY;
              })
            )
          )
        )
      ),

    })
  )
);