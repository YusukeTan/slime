<?php
/**
 * @var \App\View\AppView $this
 * @var \App\Model\Entity\Ranking[]|\Cake\Collection\CollectionInterface $ranking
 */
?>
<nav class="large-3 medium-4 columns" id="actions-sidebar">
    <ul class="side-nav">
        <li class="heading"><?= __('Actions') ?></li>
        <li><?= $this->Html->link(__('New Ranking'), ['action' => 'add']) ?></li>
    </ul>
</nav>
<div class="ranking index large-9 medium-8 columns content">
    <h3><?= __('Ranking') ?></h3>
    <table cellpadding="0" cellspacing="0">
        <thead>
            <tr>
                <th scope="col"><?= $this->Paginator->sort('Id') ?></th>
                <th scope="col"><?= $this->Paginator->sort('Name') ?></th>
                <th scope="col"><?= $this->Paginator->sort('Time') ?></th>
                <th scope="col"><?= $this->Paginator->sort('Date') ?></th>
                <th scope="col" class="actions"><?= __('Actions') ?></th>
            </tr>
        </thead>
        <tbody>
            <?php foreach ($ranking as $ranking): ?>
            <tr>
                <td><?= $this->Number->format($ranking->Id) ?></td>
                <td><?= h($ranking->Name) ?></td>
                <td><?= h($ranking->Time) ?></td>
                <td><?= h($ranking->Date) ?></td>
                <td class="actions">
                    <?= $this->Html->link(__('View'), ['action' => 'view', $ranking->Id]) ?>
                    <?= $this->Html->link(__('Edit'), ['action' => 'edit', $ranking->Id]) ?>
                    <?= $this->Form->postLink(__('Delete'), ['action' => 'delete', $ranking->Id], ['confirm' => __('Are you sure you want to delete # {0}?', $ranking->Id)]) ?>
                </td>
            </tr>
            <?php endforeach; ?>
        </tbody>
    </table>
    <div class="paginator">
        <ul class="pagination">
            <?= $this->Paginator->first('<< ' . __('first')) ?>
            <?= $this->Paginator->prev('< ' . __('previous')) ?>
            <?= $this->Paginator->numbers() ?>
            <?= $this->Paginator->next(__('next') . ' >') ?>
            <?= $this->Paginator->last(__('last') . ' >>') ?>
        </ul>
        <p><?= $this->Paginator->counter(['format' => __('Page {{page}} of {{pages}}, showing {{current}} record(s) out of {{count}} total')]) ?></p>
    </div>
</div>
